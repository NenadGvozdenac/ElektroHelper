function goToLocation(location: string) {
  window.location.href = location
}

export function goToLoginScreen() {
  goToLocation('/login')
}

export function goToDashboard() {
  goToLocation('/dashboard')
}

export function goToHome() {
  goToLocation('/')
}

export function goToForums() {
  goToLocation('/forums')
}