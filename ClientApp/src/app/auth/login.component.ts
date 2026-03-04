import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService, LoginRequest } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  form!: FormGroup;
  loading = false;
  submitted = false;
  returnUrl = '';

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/vehicles';
  }

  get f() {
    return this.form.controls;
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.form.invalid) {
      return;
    }

    this.loading = true;

    const credentials: LoginRequest = {
      email: this.f['email'].value,
      password: this.f['password'].value,
    };

    this.authService.login(credentials).subscribe({
      next: (response) => {
        if (response.success) {
          this.toastr.success('Login realizado com sucesso!');
          this.router.navigateByUrl(this.returnUrl);
        } else {
          this.toastr.error(response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        this.toastr.error(error.error?.message || 'Erro ao fazer login');
        this.loading = false;
      },
    });
  }
}
