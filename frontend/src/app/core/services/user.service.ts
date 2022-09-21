import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { environment } from "../../../environments/environment";
import { User } from "../models/user.interface";

@Injectable()
export class UserService {
  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<User[]>(environment.apiEndpoint + "/account/register").pipe(
      map((users: any) => {
        return users.data;
      })
    );
  }

  getById(_id: string) {
    return this.http.get<User>(environment.apiEndpoint + "/user/" + _id).pipe(
      map((user: any) => {
        return user.data;
      })
    );
  }
  getCurrentUser(): User {
    if (localStorage.getItem("currentUser")) {
      const user = JSON.parse(localStorage.getItem("currentUser"));
      return user;
    }
  }

  getCurrentUserProfile() {
    return this.http.get<any>(environment.apiEndpoint + "/account/profile").pipe(
      map((user: any) => {
        return user;
      })
    );
  }

  create(user: User) {
    user.email = user.username;
    return this.http.post(environment.apiEndpoint + "/account/register", user);
  }

  update(user: User) {
    return this.http.put<User>(environment.apiEndpoint + "/user/" + user._id, user).pipe(
      map((user: any) => {
        return user.data;
      })
    );
  }

  changePassword(id: string, password: any) {
    return this.http.put(environment.apiEndpoint + "/user/changepassword/" + id, { password: password }).pipe(map((res: any) => res.data));
  }

  delete(_id: string) {
    return this.http.delete(environment.apiEndpoint + "/user/" + _id);
  }
}
