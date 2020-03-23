import { Component } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Component({
  selector: 'task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.scss']
})
export class TaskComponent {
  api: string = "http://localhost:56410/";
  defaultArray: string[] = ["1", "2", "0", "2", "0", "2", "0"];
  taskArray: string[] = [];
  valueRegex = new RegExp("(-?[0-9]+)");
  isInvalid: boolean = false;
  isLoading: boolean = false;
  efficientPaths: Array<Array<PathNode>>;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.taskArray = this.defaultArray;
  }


  onValueChange(insertedValue: string) {
    this.taskArray = insertedValue.split(" ")
    this.isInvalid = false;
    for (let val of this.taskArray) {
      if (!this.valueRegex.test(val)) {
        debugger;
        this.isInvalid = true;
      }
    }
  }

  getCurrentArray() {
    return this.taskArray.join(" ");
  }

  getEfficientPaths() : Observable<any>{

      return new Observable(observer => {
        let params = new HttpParams();
        params = params.append('jumps', this.taskArray.join(', '));
        this.isLoading = true;
        var url = this.api + "Tasks/EfficientPaths";
        this.http.get<any>(url, {observe: 'response', params: params }).subscribe(
          response => {
            observer.next(response);
            observer.complete();
          }
        )
      });
  }

  getIsReachable() {
    this.isLoading = true;
    this.getEfficientPaths().subscribe(result=>{
      this.isLoading = false;
      if (result.body && result.body instanceof Array && result.body.length > 0){
        alert("Goal is reachable");
      }
      else {
        alert("Goal is unreachable");
      }
    })
  }

  getMostEfficient() {
    this.isLoading = true;

    this.efficientPaths = null;
    this.getEfficientPaths().subscribe(result=>{
      this.isLoading = false;
      if (result.body && result.body instanceof Array && result.body.length > 0){
        debugger;
        this.efficientPaths = result.body;
      }
      else {
        alert("Goal is unreachable");
      }
    })
  }
}
export class PathNode {
    Value: number;
    Index: number;
}



