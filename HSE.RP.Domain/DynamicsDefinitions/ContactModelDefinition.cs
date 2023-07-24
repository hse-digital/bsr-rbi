using HSE.RP.Domain.Entities;

namespace HSE.RP.Domain.DynamicsDefinitions;

public class ContactModelDefinition : DynamicsModelDefinition<Contact, DynamicsContact>
{
    public override string Endpoint => "contacts";

    public override DynamicsContact BuildDynamicsEntity(Contact entity)
    {
        return new DynamicsContact(firstname: entity.FirstName,lastname: entity.LastName,telephone1: entity.PhoneNumber,emailaddress1: entity.Email, contactid: entity.Id, jobRoleReferenceId: entity.jobRoleReferenceId);
    }

    public override Contact BuildEntity(DynamicsContact dynamicsEntity)
    {
        return new Contact(FirstName: dynamicsEntity.firstname, LastName: dynamicsEntity.lastname, PhoneNumber: dynamicsEntity.telephone1, Email: dynamicsEntity.emailaddress1, Id: dynamicsEntity.contactid, jobRoleReferenceId: dynamicsEntity.jobRoleReferenceId);
    }
}