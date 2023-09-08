import { ViewportScroller } from '@angular/common';
import { AfterViewInit, Component, ElementRef, Input, ViewChild, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'govuk-summary-error-linkable',
  template: `<li><a (click)="scrollToAnchor()" (keyup.enter)="scrollToAnchor()" tabindex="0" role="link">{{message}}</a><a href="{{linkUri}}" target="_blank">{{linkText}}</a></li>`
})
export class GovukSummaryErrorLinkableComponent {
  @Input() anchorId?: string;
  @Input() message?: string;
  @Input() linkText?: string;
  @Input() linkUri?: string;

  constructor(private viewportScroller: ViewportScroller) {}

  scrollToAnchor() {
    if (this.anchorId) {
      this.viewportScroller.scrollToAnchor(this.anchorId);
    }
  }
}


@Component({
  selector: 'govuk-error-linkable',
  templateUrl: './govuk-error-linkable.component.html',
  styleUrls: ['./govuk-error-linkable.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class GovukErrorLinkableComponent {
  @Input() public id?: string;
  @Input() public text?: string;
  @Input() public class?: string;
  @Input() linkText?: string;
  @Input() linkUri?: string;
}
