import { Component } from "@angular/core";
import { ComponentCompletionState } from "./component-completion-state.enum"
import { IComponentModel } from "./component. interface";

export class CompetenceyAssessmentCertificateNumber implements IComponentModel {
  CertificateNumber?: string = "";
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
}