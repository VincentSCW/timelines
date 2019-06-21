import { Routes, RouterModule } from "@angular/router";
import { AuthGuard } from "../services/auth-guard.service";
import { RecordEditorComponent } from "./record-editor/record-editor.component";
import { RecordViewComponent } from "./record-view/record-view.component";
import { NgModule } from "@angular/core";

const recordRoutes: Routes = [
  {
    path: 'records',
    children: [
      {
        path: 'create',
        component: RecordEditorComponent,
        canActivate: [AuthGuard]
      },
      {
        path: ':record/edit',
        component: RecordEditorComponent,
        canActivate: [AuthGuard]
      },
      {
        path: ':record',
        component: RecordViewComponent
      }
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(recordRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class RecordRoutingModule { }