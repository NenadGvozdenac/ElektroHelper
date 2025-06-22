<template>
    <div class="min-h-screen bg-slate-100">
        <!-- Header -->
        <header class="bg-white shadow-sm sticky top-0 z-50">
            <div class="border-b border-slate-200">
                <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-20">
                    <div class="flex items-center justify-between h-full">
                        <!-- Back Button -->
                        <button @click="goBack"
                            class="flex items-center space-x-2 text-slate-600 hover:text-emerald-600 font-medium transition-colors">
                            <ArrowLeft class="w-5 h-5" />
                            <span>Back</span>
                        </button>

                        <!-- Logo/Brand -->
                        <span @click="goBack"
                            class="text-2xl font-bold bg-gradient-to-r from-emerald-600 to-indigo-600 bg-clip-text text-transparent cursor-pointer">
                            ElektroHelper Payments
                        </span>

                        <!-- Empty div for spacing -->
                        <div class="w-16"></div>
                    </div>
                </div>
            </div>
        </header>

        <main class="container mx-auto px-4 py-6">
            <div class="max-w-7xl mx-auto">
                <!-- Page Title -->
                <div class="mb-8">
                    <h1 class="text-3xl font-bold text-slate-900">Payment Management</h1>
                    <p class="text-slate-600 mt-2">Make secure payments via PayPal or credit card</p>
                </div>

                <!-- Side by Side Layout -->
                <div class="grid grid-cols-1 xl:grid-cols-2 gap-8">
                    <!-- Left Side - Payment Form -->
                    <div class="space-y-6">
                        <div class="bg-white rounded-xl shadow-sm border border-slate-200">
                            <div class="p-6 border-b border-slate-200">
                                <div class="flex items-center space-x-3">
                                    <div
                                        class="w-10 h-10 rounded-full bg-gradient-to-br from-emerald-500 to-indigo-500 flex items-center justify-center">
                                        <CreditCard class="w-5 h-5 text-white" />
                                    </div>
                                    <div>
                                        <h3 class="text-lg font-semibold text-slate-900">Make Payment</h3>
                                        <p class="text-slate-600 text-sm">Enter payment details and choose your
                                            preferred payment method</p>
                                    </div>
                                </div>
                            </div>
                            <div class="p-6">
                                <form class="space-y-6" @submit.prevent>
                                    <!-- Row 1: Amount with Currency and Payment Purpose -->
                                    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                                        <div>
                                            <label class="block text-sm font-medium text-slate-700 mb-2">Amount</label>
                                            <div
                                                class="flex border border-slate-300 rounded-lg focus-within:ring-2 focus-within:ring-emerald-500 focus-within:border-emerald-500">
                                                <div class="relative flex-1">
                                                    <!-- Dynamic currency icon -->
                                                    <component :is="getCurrencyIcon()"
                                                        class="absolute left-3 top-3 w-5 h-5 text-slate-400" />
                                                    <input v-model="paymentForm.amount" type="number" step="0.01"
                                                        required placeholder="0.00" min="0.01"
                                                        class="w-full pl-10 pr-4 py-3 border-0 rounded-l-lg focus:ring-0 focus:border-0 outline-none" />
                                                </div>
                                                <select v-model="paymentForm.currency" @change="resetPaypal"
                                                    class="px-4 py-3 border-0 rounded-r-lg focus:ring-0 focus:border-0 bg-white outline-none border-l border-slate-200">
                                                    <option value="USD">USD</option>
                                                    <option value="EUR">EUR</option>
                                                </select>
                                            </div>
                                        </div>

                                        <div>
                                            <label class="block text-sm font-medium text-slate-700 mb-2">Payment
                                                Purpose</label>
                                            <div class="relative">
                                                <FileText class="absolute left-3 top-3 w-5 h-5 text-slate-400" />
                                                <input v-model="paymentForm.paymentPurpose" type="text" required
                                                    placeholder="Purpose of payment"
                                                    class="w-full pl-10 pr-4 py-3 border border-slate-300 rounded-lg focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500" />
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Row 2: Payee and Account Number -->
                                    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                                        <div>
                                            <label class="block text-sm font-medium text-slate-700 mb-2">Payee</label>
                                            <div class="relative">
                                                <Mail class="absolute left-3 top-3 w-5 h-5 text-slate-400" />
                                                <input v-model="paymentForm.payee" type="text" required
                                                    placeholder="Recipient name"
                                                    class="w-full pl-10 pr-4 py-3 border border-slate-300 rounded-lg focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500" />
                                            </div>
                                        </div>

                                        <div>
                                            <label class="block text-sm font-medium text-slate-700 mb-2">Account
                                                Number</label>
                                            <div class="relative">
                                                <Hash class="absolute left-3 top-3 w-5 h-5 text-slate-400" />
                                                <input v-model="paymentForm.payeeAccountNumber" type="text" required
                                                    placeholder="Account number"
                                                    class="w-full pl-10 pr-4 py-3 border border-slate-300 rounded-lg focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500" />
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Row 3: Reference Number and Payment Model -->
                                    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                                        <div>
                                            <label class="block text-sm font-medium text-slate-700 mb-2">Reference
                                                Number</label>
                                            <input v-model="paymentForm.referenceNumber" type="text" required
                                                placeholder="Reference number"
                                                class="w-full px-4 py-3 border border-slate-300 rounded-lg focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500" />
                                        </div>

                                        <div>
                                            <label class="block text-sm font-medium text-slate-700 mb-2">Payment
                                                Model</label>
                                            <input v-model="paymentForm.paymentModel" type="text" required
                                                placeholder="Payment model"
                                                class="w-full px-4 py-3 border border-slate-300 rounded-lg focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500" />
                                        </div>
                                    </div>

                                    <!-- Payment Method Buttons -->
                                    <div class="pt-4 border-t border-slate-200" v-if="isFormValid">
                                        <label class="block text-sm font-medium text-slate-700 mb-4">Choose Payment
                                            Method</label>
                                        <div id="paypal-button-container" class="w-full"></div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>

                    <!-- Right Side - Payment History -->
                    <div class="space-y-6">
                        <!-- Payment History -->
                        <div class="bg-white rounded-xl shadow-sm border border-slate-200">
                            <div class="p-6 border-b border-slate-200">
                                <h3 class="text-lg font-semibold text-slate-900">Payment History</h3>
                                <p class="text-slate-600 text-sm mt-1">Your recent payment transactions</p>
                            </div>
                            <div class="divide-y divide-slate-200 overflow-y-auto">
                                <div v-if="payments?.payments.length === 0" class="p-8 text-center">
                                    <div
                                        class="inline-flex items-center justify-center w-16 h-16 rounded-full bg-slate-100 mb-4">
                                        <FileText class="w-8 h-8 text-slate-400" />
                                    </div>
                                    <h3 class="text-lg font-medium text-slate-900">No Payments Found</h3>
                                    <p class="text-slate-600 mt-1">You don't have any payment history yet.</p>
                                </div>

                                <div v-for="payment in payments?.payments" :key="payment.id"
                                    class="p-4 hover:bg-slate-50 transition-colors">
                                    <div class="flex items-start justify-between">
                                        <div class="flex-1">
                                            <div class="flex items-center space-x-3 mb-2">
                                                <div
                                                    class="w-8 h-8 rounded-full bg-gradient-to-br from-emerald-500 to-teal-500 flex items-center justify-center">
                                                    <DollarSign class="w-4 h-4 text-white" />
                                                </div>
                                                <div>
                                                    <h4 class="font-medium text-slate-900">{{
                                                        formatAmount(payment.amount, payment.currency) }}</h4>
                                                    <p class="text-sm text-slate-600">{{ payment.paymentPurpose }}</p>
                                                </div>
                                            </div>

                                            <div class="grid grid-cols-1 gap-2 text-sm ml-11">
                                                <div>
                                                    <span class="text-slate-500">To:</span>
                                                    <span class="ml-2 text-slate-900">{{ payment.payee }}</span>
                                                </div>
                                                <div>
                                                    <span class="text-slate-500">Account:</span>
                                                    <span class="ml-2 text-slate-900 font-mono">{{
                                                        payment.payeeAccountNumber }}</span>
                                                </div>
                                                <div>
                                                    <span class="text-slate-500">Ref:</span>
                                                    <span class="ml-2 text-slate-900">{{ payment.referenceNumber
                                                    }}</span>
                                                </div>
                                                <div>
                                                    <span class="text-slate-500">Date:</span>
                                                    <span class="ml-2 text-slate-900">{{ formatDate(payment.createdAt)
                                                    }}</span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="ml-4 flex items-center">
                                            <div
                                                class="inline-flex items-center px-2 py-1 rounded-full text-xs font-medium bg-emerald-100 text-emerald-800">
                                                <CheckCircle class="w-3 h-3 mr-1" />
                                                Completed
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Load More Button -->
                            <div v-if="hasMorePayments" class="p-4 border-t border-slate-200 text-center">
                                <button @click="loadMorePayments" :disabled="loading"
                                    class="px-4 py-2 bg-slate-100 text-slate-700 rounded-lg hover:bg-slate-200 disabled:opacity-50 disabled:cursor-not-allowed transition-colors text-sm">
                                    <div v-if="loading" class="flex items-center justify-center">
                                        <div class="animate-spin rounded-full h-4 w-4 border-b-2 border-slate-600 mr-2">
                                        </div>
                                        Loading...
                                    </div>
                                    <span v-else>Load More</span>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </main>

        <!-- Toast Notification -->
        <div v-if="toastMessage" class="fixed bottom-4 right-4 z-50">
            <div class="bg-white border border-slate-200 rounded-lg shadow-lg p-4 flex items-center space-x-3">
                <CheckCircle class="w-5 h-5 text-emerald-500" />
                <span class="text-slate-900">{{ toastMessage }}</span>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, nextTick, watch } from 'vue';
import {
    CreditCard,
    DollarSign,
    Hash,
    FileText,
    CheckCircle,
    ArrowLeft,
    Euro,
    Mail
} from 'lucide-vue-next';
import type { MakePayment, Payments } from '@/app/models/payments/Payment';
import { PaymentService } from '@/app/services/payment_backend/payment_service';
import { goToLoginScreen } from '@/app/routes';
import { getAccessToken } from '@/app/services/backend/auth_service';
import { COUNTRY_CODES, PAYPAL_CLIENT_ID } from '@/app/services/backend/const_service';
import { UserService } from '@/app/services/backend/user_service';
import type { User } from '@/app/models/backend/user';
import type { Location } from '@/app/models/backend/location';
import { DashboardService } from '@/app/services/backend/dashboard_service';

// Reactive state
const loading = ref(false);
const toastMessage = ref('');
const hasMorePayments = ref(false);
const paypalSDKLoaded = ref(false);
const renderedPayPalButtons = ref(false);

// Unified payment form state with currency
const paymentForm = ref({
    amount: '',
    currency: 'USD', // Default to USD for better PayPal compatibility
    paymentPurpose: '',
    payee: '',
    payeeAccountNumber: '',
    referenceNumber: '',
    paymentModel: ''
});

// Payments data
const payments = ref<Payments>();

const isFormValid = computed(() => {
    return paymentForm.value.amount &&
        parseFloat(paymentForm.value.amount) > 0 &&
        paymentForm.value.currency &&
        paymentForm.value.paymentPurpose &&
        paymentForm.value.payee &&
        paymentForm.value.payeeAccountNumber &&
        paymentForm.value.referenceNumber &&
        paymentForm.value.paymentModel;
});

watch(isFormValid, async (valid) => {
    if (valid && (!paypalSDKLoaded.value || !document.getElementById('paypal-button-container')?.hasChildNodes()) && !renderedPayPalButtons.value) {
        try {
            const jwt = await getAccessToken();
            if (jwt) {
                await initializePayPal(jwt);
                renderedPayPalButtons.value = true; // Mark buttons as rendered
            }
        } catch (error) {
            console.error('Error initializing PayPal from watcher:', error);
        }
    }
});

// Methods
function getCurrencyIcon() {
    switch (paymentForm.value.currency) {
        case 'USD':
            return DollarSign;
        case 'EUR':
            return Euro;
        default:
            return DollarSign;
    }
}

declare global {
    interface Window {
        paypal: any;
    }
}

async function resetPaypal() {
    paypalSDKLoaded.value = false;
    const container = document.getElementById('paypal-button-container');
    if (container) {
        container.innerHTML = '';
    }

    renderedPayPalButtons.value = false; // Reset rendered state

    const jwt = await getAccessToken();
    if (jwt) {
        await initializePayPal(jwt);
    }
}

async function initializePayPal(jwt: string) {
    if (!jwt) {
        console.error('No JWT token available');
        return;
    }

    try {
        loading.value = true;
        await loadPayPalSDK(paymentForm.value.currency);

        // Wait for DOM update and ensure container exists
        await nextTick();

        // Add a small delay to ensure DOM is fully rendered
        setTimeout(() => {
            renderPayPalButton(jwt);
            loading.value = false;
        }, 100);

    } catch (error) {
        console.error('PayPal initialization error:', error);
        showToast('Failed to initialize PayPal. Please try again.');
        loading.value = false;
    }
}

function loadPayPalSDK(currency: string): Promise<void> {
    return new Promise((resolve, reject) => {
        // Check if PayPal SDK is already loaded
        if (window.paypal && paypalSDKLoaded.value) {
            resolve();
            return;
        }

        // Remove existing PayPal scripts to avoid conflicts
        const existingScripts = document.querySelectorAll('script[src*="paypal.com/sdk"]');
        existingScripts.forEach(script => script.remove());

        const script = document.createElement('script');
        script.src = `https://www.paypal.com/sdk/js?client-id=${PAYPAL_CLIENT_ID}&components=buttons,funding-eligibility&currency=${currency}`;
        script.onload = () => {
            paypalSDKLoaded.value = true;
            resolve();
        };
        script.onerror = () => {
            reject(new Error('Failed to load PayPal SDK'));
        };
        document.head.appendChild(script); // Use head instead of body
    });
}

async function renderPayPalButton(jwt: string) {
    const container = document.getElementById('paypal-button-container');
    if (!container) {
        console.error('PayPal container not found');
        return;
    }

    // Clear existing buttons
    container.innerHTML = '';

    // Check if PayPal SDK is available
    if (!window.paypal) {
        console.error('PayPal SDK not loaded');
        return;
    }

    const activeUser: User = await UserService.getActiveUser(jwt);
    const addresses: Location[] = await DashboardService.getLocationsForUser(jwt);

    const match = activeUser.phone.match(/^\+(\d{3})(\d+)$/);

    let country_code = '', national_number = '';

    if (match) {
        country_code = match[1];
        national_number = match[2];
    } else {
        console.warn('Phone number format is not recognized');
    }

    if (addresses.length > 0) {
        let short_address_country_code = getCountryCode(addresses[0]?.country || 'US');
        window.paypal.Buttons({
            style: {
                layout: 'vertical',
                color: 'blue',
                shape: 'rect',
                label: 'pay',
                height: 48,
                tagline: false,
            },
            createOrder: (data: any, actions: any) => {
                return actions.order.create({
                    purchase_units: [{
                        amount: {
                            value: paymentForm.value.amount,
                            currency_code: paymentForm.value.currency
                        },
                        description: paymentForm.value.paymentPurpose,
                        shipping: {
                            name: {
                                full_name: activeUser.name + ' ' + activeUser.surname
                            },
                            address: {
                                address_line_1: addresses[0] != null ? addresses[0].street + ' ' + addresses[0].number : '',
                                admin_area_2: addresses[0]?.city || '',
                                postal_code: addresses[0]?.postal_code || '',
                                country_code: short_address_country_code
                            },
                            phone: {
                                phone_type: 'MOBILE',
                                phone_number: {
                                    country_code: country_code || '1',
                                    national_number: national_number || ''
                                }
                            },
                        },
                    }],
                    payer: {
                        name: {
                            given_name: activeUser.name,
                            surname: activeUser.surname
                        },
                        email_address: activeUser.email,
                        address: {
                            address_line_1: addresses[0] != null ? addresses[0].street + ' ' + addresses[0].number : '',
                            admin_area_2: addresses[0]?.city || '',
                            postal_code: addresses[0]?.postal_code || '',
                            country_code: short_address_country_code
                        },
                        phone: {
                            phone_type: 'MOBILE',
                            phone_number: {
                                country_code: country_code || '1',
                                national_number: national_number || ''
                            }
                        },
                    },
                    application_context: {
                        shipping_preference: 'SET_PROVIDED_ADDRESS',
                        user_action: 'PAY_NOW',
                        return_url: window.location.href,
                        cancel_url: window.location.href,
                    }
                });
            },
            onApprove: async (data: any, actions: any) => {
                try {
                    const order = await actions.order.capture();
                    showToast('Payment completed successfully!');

                    let makePayment: MakePayment = {
                        amount: parseFloat(paymentForm.value.amount),
                        currency: paymentForm.value.currency,
                        paymentPurpose: paymentForm.value.paymentPurpose,
                        payee: paymentForm.value.payee,
                        payeeAccountNumber: paymentForm.value.payeeAccountNumber,
                        referenceNumber: paymentForm.value.referenceNumber,
                        paymentModel: paymentForm.value.paymentModel,
                    };

                    await PaymentService.makePayment(jwt, makePayment);

                    await fetchPayments(); // Refresh payment history

                    resetForm();
                } catch (error) {
                    console.error('Payment capture error:', error);
                    showToast('Payment processing failed');
                }
            },
            onError: (err: any) => {
                console.error('PayPal error:', err);
                showToast('PayPal payment error');
            },
            onCancel: (data: any) => {
                console.log('Payment cancelled:', data);
                showToast('Payment was cancelled');
            }
        }).render('#paypal-button-container');
    } else {
        window.paypal.Buttons({
            style: {
                layout: 'vertical',
                color: 'blue',
                shape: 'rect',
                label: 'pay',
                height: 48,
                tagline: false,
            },
            createOrder: (data: any, actions: any) => {
                return actions.order.create({
                    purchase_units: [{
                        amount: {
                            value: paymentForm.value.amount,
                            currency_code: paymentForm.value.currency
                        },
                        description: paymentForm.value.paymentPurpose,
                    }],
                    payer: {
                        name: {
                            given_name: activeUser.name,
                            surname: activeUser.surname
                        },
                        email_address: activeUser.email,
                        phone: {
                            phone_type: 'MOBILE',
                            phone_number: {
                                country_code: country_code || '1',
                                national_number: national_number || ''
                            }
                        },
                    }
                });
            },
            onApprove: async (data: any, actions: any) => {
                try {
                    const order = await actions.order.capture();
                    showToast('Payment completed successfully!');

                    let makePayment: MakePayment = {
                        amount: parseFloat(paymentForm.value.amount),
                        currency: paymentForm.value.currency,
                        paymentPurpose: paymentForm.value.paymentPurpose,
                        payee: paymentForm.value.payee,
                        payeeAccountNumber: paymentForm.value.payeeAccountNumber,
                        referenceNumber: paymentForm.value.referenceNumber,
                        paymentModel: paymentForm.value.paymentModel,
                    };

                    await PaymentService.makePayment(jwt, makePayment);

                    await fetchPayments(); // Refresh payment history

                    resetForm();
                } catch (error) {
                    console.error('Payment capture error:', error);
                    showToast('Payment processing failed');
                }
            },
            onError: (err: any) => {
                console.error('PayPal error:', err);
                showToast('PayPal payment error');
            },
            onCancel: (data: any) => {
                console.log('Payment cancelled:', data);
                showToast('Payment was cancelled');
            }
        }).render('#paypal-button-container');
    }
}

async function fetchPayments() {
    loading.value = true;
    try {
        const jwt = await getAccessToken();
        if (!jwt) {
            goToLoginScreen();
            return;
        }
        payments.value = await PaymentService.getPayments(jwt);
    } catch (error) {
        console.error('Error fetching payments:', error);
        showToast('Failed to load payment history. Please try again.');
    } finally {
        loading.value = false;
    }
}

function loadMorePayments() {
    if (loading.value || !hasMorePayments.value) return;
    fetchPayments();
}

function formatAmount(amount: number, currency: string): string {
    return new Intl.NumberFormat('en-UK', {
        style: 'currency',
        currency: currency
    }).format(amount);
}

function formatDate(dateString: Date): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-UK', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
    });
}

function showToast(message: string) {
    toastMessage.value = message;
    setTimeout(() => {
        toastMessage.value = '';
    }, 3000);
}

const goBack = () => {
    window.history.back();
};

onMounted(async () => {
    try {
        await fetchPayments();

        const jwt = await getAccessToken();
        if (jwt && isFormValid.value) {
            await initializePayPal(jwt);
        }
    } catch (error) {
        console.error('Error in onMounted:', error);
    }
});

function getCountryCode(countryName: string): string | undefined {
    const entry = COUNTRY_CODES.find(([name]) => name.toLowerCase() === countryName.toLowerCase());
    return entry ? entry[1] : undefined;
}

function resetForm() {
    paymentForm.value = {
        amount: '',
        currency: 'USD',
        paymentPurpose: '',
        payee: '',
        payeeAccountNumber: '',
        referenceNumber: '',
        paymentModel: ''
    };
    resetPaypal();
}
</script>