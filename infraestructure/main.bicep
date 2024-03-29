param environment string
param location string = resourceGroup().location
param swaLocation string = 'westeurope'
param d365lKeyPrefix string = ''
param d365EnvironmentKey string = 'SQUAD3--Dynamics--EnvironmentUrl'
param optionalAppName string = ''


var appName = (optionalAppName == '') ? environment : optionalAppName

@allowed([ 'Free', 'Standard' ])
param sku string = 'Standard'

param keyVaultSku object = {
    name: 'standard'
    family: 'A'
}

@allowed([
    'Standard_LRS'
    'Standard_GRS'
    'Standard_RAGRS'
])
param storageAccountType string = 'Standard_LRS'

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2018-11-30' = {
    name: 's118-${environment}-bsr-acs-portal-identity'
    location: location
}

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' = {
    name: 's118-${environment}-bsr-acs-kv'
    location: location
    properties: {
        enableRbacAuthorization: false
        tenantId: tenant().tenantId
        sku: keyVaultSku
        accessPolicies: [
            {
                objectId: managedIdentity.properties.principalId
                tenantId: tenant().tenantId
                permissions: {
                    secrets: [
                        'all'
                    ]
                }
            }
        ]
    }
}

resource cosmos 'Microsoft.DocumentDB/databaseAccounts@2021-04-15' = {
    name: 's118-${environment}-bsr-acs-cosmos'
    location: location
    kind: 'GlobalDocumentDB'
    properties: {
        publicNetworkAccess: 'Enabled'
        consistencyPolicy: {
            defaultConsistencyLevel: 'Session'
        }
        locations: [
            {
                locationName: location
                failoverPriority: 0
                isZoneRedundant: false
            }
        ]
        capabilities: [
            {
                name: 'EnableServerless'
            }
        ]
        databaseAccountOfferType: 'Standard'
    }
}

var databaseName = 'hseportal'
resource cosmosDB 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2022-05-15' = {
    parent: cosmos
    name: databaseName
    properties: {
        resource: {
            id: databaseName
        }
    }
}

var containerName = 'regulated_building_professions'
resource container 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2022-05-15' = {
    parent: cosmosDB
    name: containerName
    properties: {
        resource: {
            id: containerName
            partitionKey: {
                paths: [
                    '/id'
                ]
                kind: 'Hash'
            }
            defaultTtl: -1
        }
    }
}

var registerContainerName = 'regulating-professions'
resource container 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2022-05-15' = {
    parent: cosmosDB
    name: registerContainerName
    properties: {
        resource: {
            id: registerContainerName
            partitionKey: {
                paths: [
                    '/buildingProfessionType'
                ]
                kind: 'Hash'
            }
            defaultTtl: -1
        }
    }
}

var workspaceIds = {
    dev: '/subscriptions/7dd7c789-6ddc-446f-9d95-bc53bd12fbb3/resourceGroups/p102-dev-itf-acs-monitor-rg/providers/Microsoft.OperationalInsights/workspaces/p102-dev-itf-acs-monitor-log'
    qa : '/subscriptions/9c0963f0-2058-40ef-b829-9dd9ef573b5e/resourceGroups/p102-test-itf-acs-monitor-rg/providers/Microsoft.OperationalInsights/workspaces/p102-test-itf-acs-monitor-log'
    uat: '/subscriptions/9c0963f0-2058-40ef-b829-9dd9ef573b5e/resourceGroups/p102-test-itf-acs-monitor-rg/providers/Microsoft.OperationalInsights/workspaces/p102-test-itf-acs-monitor-log'
    pre: '/subscriptions/20007ca9-8c0c-4cf0-9fff-05306e45c7fe/resourceGroups/p102-prod-itf-acs-monitor-rg/providers/Microsoft.OperationalInsights/workspaces/p102-prod-itf-acs-monitor-log'
    prod: '/subscriptions/20007ca9-8c0c-4cf0-9fff-05306e45c7fe/resourceGroups/p102-prod-itf-acs-monitor-rg/providers/Microsoft.OperationalInsights/workspaces/p102-prod-itf-acs-monitor-log'
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
    name: 's118-${environment}-bsr-acs-ai'
    location: location
    kind: 'web'
    properties: {
        Application_Type: 'web'
        WorkspaceResourceId: workspaceIds[environment]
        Request_Source: 'rest'
    }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-08-01' = {
    name: 's118${environment}bsracsrpportalsa'
    location: location
    sku: {
        name: storageAccountType
    }
    kind: 'Storage'
}

resource hostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
    name: 's118-${environment}-bsr-acs-sq3swa-fa'
    location: location
    sku: {
        name: 'Y1'
        tier: 'Dynamic'
    }
    properties: {}
}

resource functionApp 'Microsoft.Web/sites@2021-03-01' = {
    name: 's118-${appName}-bsr-acs-rp-fa'
    location: location
    kind: 'functionapp'
    identity: {
        type: 'UserAssigned'
        userAssignedIdentities: {
            '${managedIdentity.id}': {}
        }
    }
    properties: {
        serverFarmId: hostingPlan.id
        keyVaultReferenceIdentity: managedIdentity.id
        siteConfig: {
            ftpsState: 'FtpsOnly'
            minTlsVersion: '1.2'
            appSettings: [
                {
                    name: 'AzureWebJobsStorage'
                    value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${az.environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
                }
                {
                    name: 'FUNCTIONS_WORKER_RUNTIME'
                    value: 'dotnet-isolated'
                }
                {
                    name: 'FUNCTIONS_EXTENSION_VERSION'
                    value: '~4'
                }
                {
                    name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
                    value: appInsights.properties.InstrumentationKey
                }
                {
                    name: 'Dynamics__EmailVerificationFlowUrl'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=Dynamics--EmailVerificationFlowUrl)'
                }
                {
                    name: 'Dynamics__EnvironmentUrl'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${d365EnvironmentKey})'
                }
                {
                    name: 'Dynamics__TenantId'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${d365lKeyPrefix}Dynamics--TenantId)'
                }
                {
                    name: 'Dynamics__ClientId'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${d365lKeyPrefix}Dynamics--ClientId)'
                }
                {
                    name: 'Dynamics__ClientSecret'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${d365lKeyPrefix}Dynamics--ClientSecret)'
                }
                {
                    name: 'Dynamics__LocalAuthorityTypeId'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${d365lKeyPrefix}Dynamics--LocalAuthorityTypeId)'
                }
                {
                    name: 'CosmosConnection'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=CosmosConnection)'
                }
                {
                    name: 'Integrations__CosmosConnection'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=CosmosConnection)'
                }
                {
                    name: 'Integrations__CosmosDatabase'
                    value: 'hseportal'
                }
                {
                    name: 'Integrations__CosmosContainer'
                    value: 'regulating-professions'
                }
                {
                    name: 'Integrations__OrdnanceSurveyEndpoint'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=Integrations--OrdnanceSurveyEndpoint)'
                }
                {
                    name: 'Integrations__OrdnanceSurveyApiKey'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=Integrations--OrdnanceSurveyApiKey)'
                }
                {
                    name: 'Integrations__CompaniesHouseEndpoint'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=Integrations--CompaniesHouseEndpoint)'
                }
                {
                    name: 'Integrations__CompaniesHouseApiKey'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=Integrations--CompaniesHouseApiKey)'
                }
                {
                    name: 'Integrations__PaymentEndpoint'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=Integrations--PaymentEndpoint)'
                }
                {
                    name: 'Integrations__PaymentApiKey'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=Integrations--PaymentApiKey)'
                }
                {
                    name: 'Integrations__PaymentAmount'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=RBI--PaymentAmount)'
                }
                {
                    name: 'Integrations__Environment'
                    value: '${environment}'
                }
                {
                    name: 'Integrations__NotificationServiceApiKey'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=Integrations--NotificationServiceApiKey)'
                }                
                {
                    name: 'Integrations__NotificationsAPIEndpoint'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=Integrations--NotificationsAPIEndpoint)'
                }
                {
                    name: 'Integrations__NotificationServiceOTPEmailTemplateId'
                    value: 'a866ddd1-2e02-4055-b2df-34b468da634b'
                }                
                {
                    name: 'Integrations__NotificationServiceOTPSmsTemplateId'
                    value: '0a8fc6a7-cea1-4d7b-9e81-7ef8937751db'
                }
                {
                    name: 'Swa__Url'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=SQUAD3--Swa--Url)'
                }
                {
                    name: 'Feature__DisableOtpValidation'
                    value: 'false'
                }
                                {
                    name: 'Feature__DisableApplicationDuplicationCheck'
                    value: 'false'
                }
                

                {
                    name: 'Integrations__CommonAPIEndpoint'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=Integrations--CommonAPIEndpoint)'
                }
                {
                    name: 'Integrations__CommonAPIKey'
                    value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=Integrations--CommonAPIKey)'
                }
            ]
        }
        httpsOnly: true
    }
}

resource swa 'Microsoft.Web/staticSites@2022-03-01' = {
    name: 's118-${appName}-bsr-acs-rp-swa'
    location: swaLocation
    tags: null
    properties: {
    }
    identity: {
        type: 'UserAssigned'
        userAssignedIdentities: {
            '${managedIdentity.id}': {}
        }
    }
    sku: {
        name: sku
        size: sku
    }
}

resource swaAppSettings 'Microsoft.Web/staticSites/config@2022-03-01' = {
    name: 'appsettings'
    kind: 'string'
    parent: swa
    properties: {
        APPINSIGHTS_INSTRUMENTATIONKEY: appInsights.properties.InstrumentationKey
    }
}

resource swaFunctionAppLink 'Microsoft.Web/staticSites/userProvidedFunctionApps@2022-03-01' = {
    name: 's118-${appName}-bsr-acs-rp-swa-fa'
    parent: swa
    properties: {
        functionAppRegion: functionApp.location
        functionAppResourceId: functionApp.id
    }
}
