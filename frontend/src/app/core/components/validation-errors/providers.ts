import { InjectionToken, ValueSansProvider, FactorySansProvider, Type } from '@angular/core';
import { ErrorsMap } from './types';
import { ValidationErrorComponent } from './Validation-messages.component';

export const FORM_ERRORS = new InjectionToken('FORM_ERRORS', {
  providedIn: 'root',
  factory: () => {
    return {};
  }
});

export interface ErrorsUseValue extends ValueSansProvider {
  useValue: ErrorsMap;
}

export interface ErrorsUseFactory extends FactorySansProvider {
  useFactory: (...args: any[]) => ErrorsMap;
}

export type ErrorsProvider = ErrorsUseValue | ErrorsUseFactory;

export type ControlErrorConfig = {
  errors?: ErrorsProvider;
  blurPredicate?: (element: Element) => boolean;
  controlErrorComponent?: Type<ValidationErrorComponent>;
  controlErrorComponentAnchorFn?: (hostElement: Element, errorElement: Element) => () => void;
};

export const ControlErrorConfigProvider = new InjectionToken<ControlErrorConfig>('ControlErrorConfigProvider');
