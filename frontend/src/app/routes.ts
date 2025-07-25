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

export function goToForum(forumId: string) {
  goToLocation(`/forums/${forumId}`)
}

export function goToPost(postId: string) {
  goToLocation(`/posts/${postId}`)
}

export function goToProfile(userId: string | number) {
  goToLocation(`/profile/${userId}`)
}

export function goToRss() {
  goToLocation('/rss')
}

export function goToPayments() {
  goToLocation('/payments')
}