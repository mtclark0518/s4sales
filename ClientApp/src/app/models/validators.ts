import { ValidatorFn, AbstractControl } from '@angular/forms';

export class CustomValidators {

  public pwValidator(reqs: RegExp): ValidatorFn {
    return (control: AbstractControl): {[key: string]: any} => {
      const valid = reqs.test(control.value);
      return valid ? null : {'invalidPassword': {value: control.value}};
    };
  }

}

