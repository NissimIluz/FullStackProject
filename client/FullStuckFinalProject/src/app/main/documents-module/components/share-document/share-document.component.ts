import { Component, Inject } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
@Component({
  selector: 'share-document',
  templateUrl: './share-document.component.html',
  styleUrls: ['./share-document.component.css']
})
export class ShareDocumentComponent {
  title: string = "Share with user";
  actionButtonText = "Add";
  cancelButtonText = "Cancel";
  myControl = new FormControl();
  options: string[];
  filteredOptions: Observable<string[]>;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<ShareDocumentComponent>) {
    this.options = data.users;
    this.filteredOptions = this.myControl.valueChanges.pipe(
      startWith(''),
      map(value => typeof value === 'string' ? value : value.userId),
      map(name => name ? this._filter(name) : this.options.slice())
    );
  }
  private _filter(name: string): string[] {
    const filterValue = name.toLowerCase();

    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }
  GetUser(user: string): string {
    return user;
  }

  AddShare(userId: string) {
    this.dialogRef.close(userId)
  }
}