import { HttpInterceptorFn } from '@angular/common/http';

export const JwtInterceptor: HttpInterceptorFn = (request, next) => {

  if (typeof window !== 'undefined') {
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.jwToken) {
      console.log('currentUser.jwToken', currentUser.jwToken);
        request = request.clone({
            setHeaders: {
                Authorization: `Bearer ${currentUser.jwToken}`,
            },
        });
    }
}

return next(request);
};
