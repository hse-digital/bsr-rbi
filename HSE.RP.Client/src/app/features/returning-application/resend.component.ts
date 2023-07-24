import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ApplicationService } from "src/app/services/application.service";

@Component({
    selector: 'application-resend',
    templateUrl: './resend.component.html'
})
export class ReturningApplicationResendCodeComponent {

  @Input() emailAddress?: string;
  @Input() applicationNumber!: string;
  @Input() verificationOption!: string;
  @Input() phoneNumber?: string;
    @Output() onVerificationCodeSent = new EventEmitter();

    sendingRequest = false;

    constructor(private applicationService: ApplicationService) { }

    async sendNewCode() {
        this.sendingRequest = true;
        if(this.verificationOption === 'email') {
        await this.applicationService.sendVerificationEmail(this.emailAddress!);
        } else {
        await this.applicationService.sendVerificationSms(this.phoneNumber!);
        }
        this.onVerificationCodeSent.emit();
    }
}
