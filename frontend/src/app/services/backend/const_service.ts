const API_URL = 'http://localhost/api'
const FORUM_URL = 'http://localhost/forums-api/api'
const PAYMENT_URL = 'http://localhost/payment-api/api'
const PAYPAL_CLIENT_ID: string = import.meta.env.VITE_PAYPAL_CLIENT_ID || 'your-paypal-client-id';

const COUNTRY_CODES: string[][] = [
    ['Serbia', 'RS'],
    ['United States', 'US'],
    ['United Kingdom', 'GB'],
    ['Germany', 'DE'],
    ['France', 'FR'],
    ['Italy', 'IT'],
    ['Spain', 'ES'],
    ['Canada', 'CA'],
    ['Australia', 'AU'],
    ['Japan', 'JP'],
    ['China', 'CN'],
    ['India', 'IN'],
    ['Brazil', 'BR'],
    ['Russia', 'RU'],
    ['South Korea', 'KR'],
]

export { API_URL, FORUM_URL, PAYMENT_URL, PAYPAL_CLIENT_ID, COUNTRY_CODES };