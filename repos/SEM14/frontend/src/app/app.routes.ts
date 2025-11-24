import { Routes } from '@angular/router';
import { ArticulosList } from './components/articulos-list/articulos-list';
import { TipoArticulosList } from './components/tipo-articulos-list/tipo-articulos-list';

export const routes: Routes = [
  { path: '', redirectTo: '/articulos', pathMatch: 'full' },
  { path: 'articulos', component: ArticulosList },
  { path: 'tipos', component: TipoArticulosList }
];
