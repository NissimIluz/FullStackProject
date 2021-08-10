import { ElementRef, Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { debounceTime } from 'rxjs/operators';
import { CommService } from './comm.service';
import { UserProperties } from 'src/app/user-properties.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})

export class UserService {
  constructor(private commService: CommService, private userProp: UserProperties, private router: Router) { }
  invalidMassageName = " "
  invalidMassageID = " "
  serverDTO = ""
  currentAction = "Login"
  alternativeAction = "Register"
  nameInput!: ElementRef<any>
  idInput!: ElementRef<any>

  Init(userFormData: FormGroup, nameInput: ElementRef<any>, idInput: ElementRef<any>): void {
    this.nameInput = nameInput
    this.idInput = idInput
    var userNameChange$ = userFormData.controls.userName.valueChanges.pipe(debounceTime(1000))
    var userIDChange$ = userFormData.controls.userID.valueChanges.pipe(debounceTime(1000))
    userNameChange$.subscribe(x => this.nameVisualConfig(userFormData.controls.userName.valid, nameInput))
    userIDChange$.subscribe(v => this.idVisualConfig(userFormData, idInput))
  }
  private nameVisualConfig(valid: boolean, nameInput: any): void {
    if (valid) {
      this.invalidMassageName = " "
      nameInput.nativeElement.classList.add("inputValid")
    }
    else {
      this.invalidMassageName = "Full name required"
      nameInput.nativeElement.classList.add("inputInvalid")
    }
  }
  private idVisualConfig(userFormData: any, userID: any): void {
    if (userFormData.controls.userID.valid) {
      this.invalidMassageID = " "
      userID.nativeElement.classList.remove("inputInvalid")
    }
    else {
      this.invalidMassageID = this.emailInvalidCheck(userFormData.controls.userID.value)
      userID.nativeElement.classList.add("inputInvalid")
    }
  }
  SendForm(userFormData: FormGroup) {
    this.serverDTO = ""
    var postdate = this.commService.User(this.currentAction, userFormData)
    postdate.subscribe((response: any) => this.userResponseDTO(response.succeed, response.message, userFormData),
      error => this.userResponseDTO(false, error.message, userFormData))
  }
  private userResponseDTO(succeed: boolean, message: string, userFormData: FormGroup): void {
    console.log(message)
    if (succeed) {
      this.userProp.SetUserProperties(userFormData.controls.userID.value, userFormData.controls.userName.value)
      this.router.navigate(["main/documents"])
    }
    else {
      this.serverDTO = message
      this.nameInput.nativeElement.classList.add("inputInvalid")
      this.idInput.nativeElement.classList.add("inputInvalid")
    }
  }

  ChangeAction() {
    this.serverDTO = ""
    var temp = this.currentAction
    this.currentAction = this.alternativeAction
    this.alternativeAction = temp
    this.idInput.nativeElement.classList.remove("inputInvalid")
    this.nameInput.nativeElement.classList.remove("inputInvalid")

  }

  get ServerDTO(): string {
    return this.serverDTO
  }
  private emailInvalidCheck(value: string): string {
    return value == "" ? "Email required" : "Invalid email address"
  }

  Unsubscribe(): void {
    this.commService.Unsubscribe(this.userProp.UserID).subscribe((response: any) => this.serverResponse(response.succeed, response.message),
      error => this.serverResponse(false, error))
  }

  private serverResponse(succeed: boolean, message: string): void {
    if (succeed) {
      this.userProp.Logout();
      this.router.navigate([""])
    }
    else {
      window.alert(message)
    }
  }
}

