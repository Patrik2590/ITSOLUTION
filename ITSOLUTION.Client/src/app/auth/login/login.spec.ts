import { ComponentFixture, TestBed } from '@angular/core/testing';
// 1. Cambiamos Login por LoginComponent
import { LoginComponent } from './login'; 

describe('LoginComponent', () => { // 2. Actualizamos el nombre aquí
  let component: LoginComponent;     // 3. Y aquí
  let fixture: ComponentFixture<LoginComponent>; // 4. Y aquí

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginComponent],     // 5. Y aquí
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent); // 6. Y aquí
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});