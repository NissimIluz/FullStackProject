import { Injectable } from '@angular/core';
import {webSocket} from 'rxjs/webSocket'

@Injectable({
  providedIn: 'root'
})

export class WebSocketService {

  constructor() { }

  Connect(id:string){
    var url = "wss://localhost:44326/ws?id="+id
    var obs$ = webSocket({url:url,deserializer:msg=>msg})
    return obs$
  }

}
