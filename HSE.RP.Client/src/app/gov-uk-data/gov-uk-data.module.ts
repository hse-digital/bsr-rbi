import { RouterModule, Routes } from '@angular/router';
import { GovUKDataComponent } from './gov-uk-data.component';
import { NgModule } from '@angular/core';

const routes: Routes = [
  { path: GovUKDataComponent.route, component: GovUKDataComponent },
];

@NgModule({
  declarations: [GovUKDataComponent],
  imports: [RouterModule.forChild(routes)]
})
export class GovUKDataModule {}
