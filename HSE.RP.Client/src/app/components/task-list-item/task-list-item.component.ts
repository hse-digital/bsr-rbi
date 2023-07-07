import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
  selector: 'hse-task-list-item',
  templateUrl: './task-list-item.component.html'
})
export class TaskListItemComponent {
    @Input() title?: string;
    @Input() canStart: boolean = false;
    @Input() inProgress: boolean = true;
    @Input() isComplete: boolean = false;
    @Output() navigate = new EventEmitter();

    linkClicked()  {
      this.navigate.emit();
    }

}