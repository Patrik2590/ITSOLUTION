export interface Tab {
  id: string;        // Usualmente la ruta principal, ej: '/tickets'
  label: string;     // Nombre visible en la pestaña
  icon: string;      // Ícono de PrimeIcons
  route: string;     // URL completa a la que navegar
  active: boolean;   // Estado visual de la pestaña
}