const API_URL: string = import.meta.env.API_URL || 'http://localhost:8080/api';
const FORUM_URL: string = import.meta.env.FORUM_URL || 'http://localhost:9090/api';
const PAYMENT_URL: string = import.meta.env.PAYMENT_URL || 'http://localhost:9091/api';

const PAYPAL_CLIENT_ID: string = import.meta.env.PAYPAL_CLIENT_ID || 'your-paypal-client-id';

export { API_URL, FORUM_URL, PAYMENT_URL, PAYPAL_CLIENT_ID };