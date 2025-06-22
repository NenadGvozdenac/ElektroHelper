<template>
    <div class="min-h-screen bg-slate-100">
        <Navbar />

        <main class="container mx-auto px-4 pt-24">
            <div class="max-w-7xl mx-auto">
                <!-- Hero Section -->
                <div class="text-center mb-16">
                    <h1 class="text-5xl font-bold text-slate-900 mb-6">
                        Revolutionizing
                        <span class="bg-gradient-to-r from-emerald-600 to-indigo-600 bg-clip-text text-transparent">
                            Property Management
                        </span>
                    </h1>
                    <p class="text-xl text-slate-600 max-w-4xl mx-auto leading-relaxed">
                        ElektroHelper is a comprehensive platform that combines location tracking, electricity
                        monitoring,
                        community engagement, and seamless payment processing into one powerful solution. Designed for
                        property managers, homeowners, and communities who demand efficiency and transparency.
                    </p>
                </div>

                <!-- Statistics -->
                <div class="grid grid-cols-2 lg:grid-cols-4 gap-6 mb-16">
                    <div v-for="stat in stats" :key="stat.label"
                        class="bg-white rounded-xl shadow-sm border border-slate-200 p-6 text-center">
                        <div
                            class="w-12 h-12 rounded-full bg-gradient-to-br from-emerald-500 to-indigo-500 flex items-center justify-center mx-auto mb-4">
                            <component :is="stat.icon" class="w-6 h-6 text-white" />
                        </div>
                        <div class="text-3xl font-bold text-slate-900 mb-2">{{ stat.number }}</div>
                        <div class="text-slate-600 text-sm">{{ stat.label }}</div>
                    </div>
                </div>

                <!-- Main Applications Overview -->
                <div class="mb-16">
                    <h2 class="text-3xl font-bold text-slate-900 text-center mb-12">Three Powerful Applications, One
                        Platform</h2>

                    <!-- Application 1: Main Tracking App -->
                    <div class="bg-white rounded-xl shadow-sm border border-slate-200 mb-8">
                        <div class="p-8">
                            <div class="flex items-start">
                                <div
                                    class="w-16 h-16 rounded-full bg-gradient-to-br from-emerald-500 to-teal-500 flex items-center justify-center mr-6 flex-shrink-0">
                                    <Zap class="w-8 h-8 text-white" />
                                </div>
                                <div class="flex-1">
                                    <h3 class="text-2xl font-bold text-slate-900 mb-4">Property & Electricity Management
                                    </h3>
                                    <p class="text-slate-600 mb-6 text-lg leading-relaxed">
                                        Our flagship application provides comprehensive property management capabilities
                                        with advanced
                                        electricity monitoring. Track multiple locations, monitor meter readings in
                                        real-time, and
                                        manage payments for individual homes with unprecedented accuracy and efficiency.
                                    </p>

                                    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                                        <div class="space-y-4">
                                            <h4 class="font-semibold text-slate-900 text-lg">Location Management</h4>
                                            <ul class="space-y-3 text-slate-600">
                                                <li class="flex items-start">
                                                    <MapPin
                                                        class="w-5 h-5 text-emerald-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>GPS-precise property location tracking with interactive
                                                        mapping</span>
                                                </li>
                                                <li class="flex items-start">
                                                    <Building
                                                        class="w-5 h-5 text-emerald-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Multi-property portfolio management with hierarchical
                                                        organization</span>
                                                </li>
                                                <li class="flex items-start">
                                                    <Eye class="w-5 h-5 text-emerald-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Real-time property status monitoring and automated
                                                        alerts</span>
                                                </li>
                                            </ul>
                                        </div>

                                        <div class="space-y-4">
                                            <h4 class="font-semibold text-slate-900 text-lg">Electricity Monitoring</h4>
                                            <ul class="space-y-3 text-slate-600">
                                                <li class="flex items-start">
                                                    <Activity
                                                        class="w-5 h-5 text-indigo-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Smart meter integration with automated reading
                                                        collection</span>
                                                </li>
                                                <li class="flex items-start">
                                                    <BarChart3
                                                        class="w-5 h-5 text-indigo-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Advanced consumption analytics with trend forecasting</span>
                                                </li>
                                                <li class="flex items-start">
                                                    <TrendingUp
                                                        class="w-5 h-5 text-indigo-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Individual home payment calculations and billing
                                                        automation</span>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Application 2: Forums -->
                    <div class="bg-white rounded-xl shadow-sm border border-slate-200 mb-8">
                        <div class="p-8">
                            <div class="flex items-start">
                                <div
                                    class="w-16 h-16 rounded-full bg-gradient-to-br from-blue-500 to-purple-500 flex items-center justify-center mr-6 flex-shrink-0">
                                    <MessageSquare class="w-8 h-8 text-white" />
                                </div>
                                <div class="flex-1">
                                    <h3 class="text-2xl font-bold text-slate-900 mb-4">Community Forums & Communication
                                    </h3>
                                    <p class="text-slate-600 mb-6 text-lg leading-relaxed">
                                        Foster community engagement and streamline communication with our integrated
                                        forum system.
                                        Property managers, residents, and stakeholders can collaborate effectively
                                        through organized
                                        discussions, knowledge sharing, and community-driven problem solving.
                                    </p>

                                    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                                        <div class="space-y-4">
                                            <h4 class="font-semibold text-slate-900 text-lg">Discussion Features</h4>
                                            <ul class="space-y-3 text-slate-600">
                                                <li class="flex items-start">
                                                    <FileText class="w-5 h-5 text-blue-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Organized forum categories for different property-related
                                                        topics</span>
                                                </li>
                                                <li class="flex items-start">
                                                    <MessageSquare
                                                        class="w-5 h-5 text-blue-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Threaded discussions with nested comments and replies</span>
                                                </li>
                                                <li class="flex items-start">
                                                    <Users class="w-5 h-5 text-blue-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>User profiles with reputation systems and community
                                                        badges</span>
                                                </li>
                                            </ul>
                                        </div>

                                        <div class="space-y-4">
                                            <h4 class="font-semibold text-slate-900 text-lg">Engagement Tools</h4>
                                            <ul class="space-y-3 text-slate-600">
                                                <li class="flex items-start">
                                                    <ThumbsUp
                                                        class="w-5 h-5 text-purple-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Upvote and downvote system for quality content curation</span>
                                                </li>
                                                <li class="flex items-start">
                                                    <Star class="w-5 h-5 text-purple-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Content moderation tools and community guidelines
                                                        enforcement</span>
                                                </li>
                                                <li class="flex items-start">
                                                    <Globe class="w-5 h-5 text-purple-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Real-time notifications and activity feeds for community
                                                        updates</span>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Application 3: Payments -->
                    <div class="bg-white rounded-xl shadow-sm border border-slate-200 mb-8">
                        <div class="p-8">
                            <div class="flex items-start">
                                <div
                                    class="w-16 h-16 rounded-full bg-gradient-to-br from-green-500 to-emerald-500 flex items-center justify-center mr-6 flex-shrink-0">
                                    <CreditCard class="w-8 h-8 text-white" />
                                </div>
                                <div class="flex-1">
                                    <h3 class="text-2xl font-bold text-slate-900 mb-4">Secure Payment Processing</h3>
                                    <p class="text-slate-600 mb-6 text-lg leading-relaxed">
                                        Streamline financial transactions with our integrated payment system supporting
                                        multiple
                                        payment methods. Users can securely process payments via PayPal and credit cards
                                        while
                                        maintaining complete transparency and detailed transaction history.
                                    </p>

                                    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                                        <div class="space-y-4">
                                            <h4 class="font-semibold text-slate-900 text-lg">Payment Methods</h4>
                                            <ul class="space-y-3 text-slate-600">
                                                <li class="flex items-start">
                                                    <DollarSign
                                                        class="w-5 h-5 text-green-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>PayPal integration with instant payment processing</span>
                                                </li>
                                                <li class="flex items-start">
                                                    <CreditCard
                                                        class="w-5 h-5 text-green-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Major credit card support with PCI DSS compliance</span>
                                                </li>
                                                <li class="flex items-start">
                                                    <Globe class="w-5 h-5 text-green-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Multi-currency support for international transactions</span>
                                                </li>
                                            </ul>
                                        </div>

                                        <div class="space-y-4">
                                            <h4 class="font-semibold text-slate-900 text-lg">Transaction Management</h4>
                                            <ul class="space-y-3 text-slate-600">
                                                <li class="flex items-start">
                                                    <FileText
                                                        class="w-5 h-5 text-emerald-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Comprehensive payment history with detailed records</span>
                                                </li>
                                                <li class="flex items-start">
                                                    <Clock class="w-5 h-5 text-emerald-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Real-time transaction status tracking and notifications</span>
                                                </li>
                                                <li class="flex items-start">
                                                    <Shield
                                                        class="w-5 h-5 text-emerald-500 mr-3 mt-0.5 flex-shrink-0" />
                                                    <span>Advanced security measures and fraud protection</span>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Key Features Grid -->
                <div class="mb-16">
                    <h2 class="text-3xl font-bold text-slate-900 text-center mb-12">Why Choose ElektroHelper?</h2>
                    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                        <div v-for="feature in features" :key="feature.title"
                            class="bg-white rounded-xl shadow-sm border border-slate-200 p-6 hover:shadow-md transition-shadow">
                            <div
                                class="w-12 h-12 rounded-full bg-gradient-to-br from-emerald-500 to-indigo-500 flex items-center justify-center mb-4">
                                <component :is="feature.icon" class="w-6 h-6 text-white" />
                            </div>
                            <h3 class="text-lg font-semibold text-slate-900 mb-3">{{ feature.title }}</h3>
                            <p class="text-slate-600 text-sm leading-relaxed">{{ feature.description }}</p>
                        </div>
                    </div>
                </div>

                <!-- Benefits Section -->
                <div class="bg-white rounded-xl shadow-sm border border-slate-200 mb-16">
                    <div class="p-8">
                        <h2 class="text-3xl font-bold text-slate-900 text-center mb-8">Comprehensive Benefits</h2>
                        <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
                            <div class="space-y-6">
                                <h3 class="text-xl font-semibold text-slate-900">For Property Managers</h3>
                                <ul class="space-y-4 text-slate-600">
                                    <li class="flex items-start">
                                        <CheckCircle class="w-5 h-5 text-emerald-500 mr-3 mt-0.5 flex-shrink-0" />
                                        <span>Centralized management of multiple properties with unified
                                            dashboard</span>
                                    </li>
                                    <li class="flex items-start">
                                        <CheckCircle class="w-5 h-5 text-emerald-500 mr-3 mt-0.5 flex-shrink-0" />
                                        <span>Automated meter readings reducing manual labor by up to 90%</span>
                                    </li>
                                    <li class="flex items-start">
                                        <CheckCircle class="w-5 h-5 text-emerald-500 mr-3 mt-0.5 flex-shrink-0" />
                                        <span>Streamlined payment collection with automated billing and reminders</span>
                                    </li>
                                    <li class="flex items-start">
                                        <CheckCircle class="w-5 h-5 text-emerald-500 mr-3 mt-0.5 flex-shrink-0" />
                                        <span>Enhanced tenant communication through integrated forum system</span>
                                    </li>
                                </ul>
                            </div>

                            <div class="space-y-6">
                                <h3 class="text-xl font-semibold text-slate-900">For Residents & Homeowners</h3>
                                <ul class="space-y-4 text-slate-600">
                                    <li class="flex items-start">
                                        <CheckCircle class="w-5 h-5 text-indigo-500 mr-3 mt-0.5 flex-shrink-0" />
                                        <span>Transparent electricity consumption tracking with detailed
                                            analytics</span>
                                    </li>
                                    <li class="flex items-start">
                                        <CheckCircle class="w-5 h-5 text-indigo-500 mr-3 mt-0.5 flex-shrink-0" />
                                        <span>Convenient payment options with secure online processing</span>
                                    </li>
                                    <li class="flex items-start">
                                        <CheckCircle class="w-5 h-5 text-indigo-500 mr-3 mt-0.5 flex-shrink-0" />
                                        <span>Community engagement platform for neighbor interaction</span>
                                    </li>
                                    <li class="flex items-start">
                                        <CheckCircle class="w-5 h-5 text-indigo-500 mr-3 mt-0.5 flex-shrink-0" />
                                        <span>Complete payment history and consumption records at your fingertips</span>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Technology Stack -->
                <div class="bg-white rounded-xl shadow-sm border border-slate-200 mb-16">
                    <div class="p-8">
                        <h2 class="text-3xl font-bold text-slate-900 text-center mb-8">Built with Modern Technology</h2>
                        <p class="text-slate-600 text-center mb-8 text-lg max-w-3xl mx-auto">
                            ElektroHelper leverages cutting-edge technologies to deliver a robust, scalable, and secure
                            platform
                            that meets the demands of modern property management and community engagement.
                        </p>

                        <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
                            <div class="text-center">
                                <div
                                    class="w-16 h-16 rounded-full bg-gradient-to-br from-blue-500 to-cyan-500 flex items-center justify-center mx-auto mb-4">
                                    <Smartphone class="w-8 h-8 text-white" />
                                </div>
                                <h3 class="text-lg font-semibold text-slate-900 mb-3">Responsive Design</h3>
                                <p class="text-slate-600">Optimized for all devices with progressive web app
                                    capabilities</p>
                            </div>

                            <div class="text-center">
                                <div
                                    class="w-16 h-16 rounded-full bg-gradient-to-br from-green-500 to-teal-500 flex items-center justify-center mx-auto mb-4">
                                    <Shield class="w-8 h-8 text-white" />
                                </div>
                                <h3 class="text-lg font-semibold text-slate-900 mb-3">Enterprise Security</h3>
                                <p class="text-slate-600">Bank-level encryption and compliance with industry standards
                                </p>
                            </div>

                            <div class="text-center">
                                <div
                                    class="w-16 h-16 rounded-full bg-gradient-to-br from-purple-500 to-pink-500 flex items-center justify-center mx-auto mb-4">
                                    <Lightbulb class="w-8 h-8 text-white" />
                                </div>
                                <h3 class="text-lg font-semibold text-slate-900 mb-3">AI-Powered Insights</h3>
                                <p class="text-slate-600">Machine learning algorithms for predictive analytics and
                                    optimization</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </main>
        <Footer />
    </div>
</template>
<script lang="ts" setup>
import { ref } from 'vue';
import {
    Zap,
    MapPin,
    CreditCard,
    MessageSquare,
    BarChart3,
    Shield,
    Users,
    TrendingUp,
    Home,
    DollarSign,
    Clock,
    CheckCircle,
    Star,
    Globe,
    Smartphone,
    Lightbulb,
    FileText,
    ThumbsUp,
    Eye,
    Building,
    Activity} from 'lucide-vue-next';

import Navbar from '@/components/Navbar.vue';
import Footer from '@/components/Footer.vue';

const features = ref([
    {
        icon: MapPin,
        title: "Location Tracking",
        description: "Monitor multiple properties and their exact locations with precision mapping technology."
    },
    {
        icon: Zap,
        title: "Smart Meter Reading",
        description: "Automated electricity meter readings with real-time consumption analytics and alerts."
    },
    {
        icon: BarChart3,
        title: "Payment Analytics",
        description: "Comprehensive payment tracking with detailed analytics and forecasting capabilities."
    },
    {
        icon: MessageSquare,
        title: "Community Forums",
        description: "Connect with neighbors and property managers through integrated discussion platforms."
    }
]);

const stats = ref([
    { number: "10,000+", label: "Active Users", icon: Users },
    { number: "50,000+", label: "Properties Tracked", icon: Home },
    { number: "99.9%", label: "Uptime Guarantee", icon: Shield },
    { number: "24/7", label: "Support Available", icon: Clock }
]);
</script>