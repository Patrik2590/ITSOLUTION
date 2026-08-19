import { Directive, Input, TemplateRef, ViewContainerRef, inject } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';

@Directive({
  selector: '[hasRole]',
  standalone: true
})
export class HasRoleDirective {
  private templateRef = inject(TemplateRef<any>);
  private viewContainer = inject(ViewContainerRef);
  private authService = inject(AuthService);

  private rolesDelUsuario: string[] = [];

  constructor() {
    this.rolesDelUsuario = this.authService.getRolesUsuario();
  }

  @Input() set hasRole(rolesRequeridos: string[]) {
    const tieneAcceso = rolesRequeridos.some(rol => this.rolesDelUsuario.includes(rol));

    if (tieneAcceso) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }
}