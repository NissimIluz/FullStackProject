import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  @ViewChild("nameInput") nameInput!: ElementRef<any>
  @ViewChild("idInput") idInput!: ElementRef<any>
  userFormData = new FormGroup({
    userName: new FormControl("", Validators.required),
    userID: new FormControl("", [Validators.required, Validators.email])
  })
  class: string = "inputInvalid"

  constructor(private userService: UserService) { }
  SendForm() {
    this.userService.SendForm(this.userFormData)
  }

  ChangeAction() {
    this.userService.ChangeAction()

  }

  ngOnInit(): void {
  }
  ngAfterViewInit(): void {
    this.userService.Init(this.userFormData, this.nameInput, this.idInput)
  }
  get InvalidMassageName(): string {
    return this.userService.invalidMassageName
  }
  get InvalidMassageID(): string {
    return this.userService.invalidMassageID
  }
  get ServerDTO(): string {
    return this.userService.ServerDTO
  }
  get CurrentAction() {
    return this.userService.currentAction
  }
  get AlternativeAction() {
    return this.userService.alternativeAction
  }
}
