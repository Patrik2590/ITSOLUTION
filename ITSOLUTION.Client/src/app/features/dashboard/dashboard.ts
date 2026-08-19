import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core'; // <--- 1. Importar ChangeDetectorRef
import { CommonModule } from '@angular/common';
import { DashboardService, DashboardData } from '../../core/services/dashboard.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html'
})
export class DashboardComponent implements OnInit, OnDestroy {
  isDarkMode: boolean = true; 
  isLoading: boolean = true;
  
  dashboardData: DashboardData | null = null;
  private dataSubscription!: Subscription;

  constructor(
    private dashboardService: DashboardService,
    private cdr: ChangeDetectorRef // <--- 2. Inyectar en el constructor
  ) {}

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData() {
    this.isLoading = true;
    this.dataSubscription = this.dashboardService.getDashboardSummary().subscribe({
      next: (data) => {
        if (!data) {
          console.warn('⚠️ No llegaron datos del backend. Usando Mock Data...');
          this.loadMockData();
        } else {
          console.log('✅ ¡Datos reales recibidos desde .NET!', data);
          this.dashboardData = data;
        }
        this.isLoading = false;
        
        // <--- 3. Avisar al HTML que se actualice inmediatamente
        this.cdr.detectChanges(); 
      },
      error: (err) => {
        console.error('❌ Error de conexión con la API de .NET:', err);
        console.warn('⚠️ Activando Mock Data de respaldo...');
        this.loadMockData(); 
        this.isLoading = false;
        
        // <--- 4. Avisar al HTML también en caso de error
        this.cdr.detectChanges(); 
      }
    });
  }

  private loadMockData() {
    this.dashboardData = {
      kpis: { systemUptime: '99.9%', activeTickets: 14, securityAlerts: 2, pendingUpdates: 8 },
      recentAlerts: [
        { id: 'TKT-1042', type: 'Critical', message: 'Fallo de conexión en Servidor DB-01', time: '10 min ago' },
        { id: 'SYS-009', type: 'Warning', message: 'Licencia de Office 365 expira en 5 días', time: '1 hour ago' },
        { id: 'TKT-1043', type: 'Info', message: 'Nuevo usuario creado: m.smith', time: '2 hours ago' }
      ]
    };
  }

  ngOnDestroy(): void {
    if (this.dataSubscription) {
      this.dataSubscription.unsubscribe(); 
    }
  }
}