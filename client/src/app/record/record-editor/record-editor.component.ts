import { Component, OnInit } from '@angular/core';
import { Record } from '../../models/record.model';

@Component({
  selector: 'app-record-editor',
  templateUrl: './record-editor.component.html',
  styleUrls: ['./record-editor.component.scss']
})
export class RecordEditorComponent implements OnInit {
  model: Record = {
    date: new Date(),
    title: '',
    imageUrl: ''
  };

  constructor() { }

  ngOnInit() {
    
  }

  onUploadClicked() {

  }

  onSubmit() {

  }

  onCancel() {
    
  }
}
