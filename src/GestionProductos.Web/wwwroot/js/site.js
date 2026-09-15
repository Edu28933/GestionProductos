const toggle = document.getElementById('menuToggle');
const sidebar = document.getElementById('sidebar');
const overlay = document.getElementById('sidebarOverlay');
function closeMenu() { sidebar?.classList.remove('open'); overlay?.classList.remove('show'); toggle?.setAttribute('aria-expanded', 'false'); }
toggle?.addEventListener('click', () => { const open = sidebar?.classList.toggle('open'); overlay?.classList.toggle('show', !!open); toggle.setAttribute('aria-expanded', String(!!open)); });
overlay?.addEventListener('click', closeMenu);
sidebar?.querySelectorAll('a').forEach(a => a.addEventListener('click', closeMenu));
