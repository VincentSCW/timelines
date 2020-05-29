import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxEditorModule } from 'ngx-editor';
import { NgxMasonryModule } from 'ngx-masonry';

import { MaterialModule } from './material.module';
import { ControlsModule } from './controls/controls.module';

@NgModule({
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    NgxEditorModule,
    FormsModule,
    HttpClientModule,
    MaterialModule,
    BrowserAnimationsModule,
    NgxEditorModule,
    ControlsModule,
    NgxMasonryModule
  ],
  exports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    NgxEditorModule,
    FormsModule,
    HttpClientModule,
    MaterialModule,
    BrowserAnimationsModule,
    NgxEditorModule,
    ControlsModule,
    NgxMasonryModule
  ]
})

export class SharedModule { }
