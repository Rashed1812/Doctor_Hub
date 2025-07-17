// WhatsApp Bot Test Script
class WhatsAppBotTester {
    constructor() {
        this.baseUrl = '/api/whatsappbot';
    }

    // Send welcome message
    async sendWelcome(phoneNumber) {
        try {
            const response = await fetch(`${this.baseUrl}/send-welcome`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ phoneNumber })
            });

            const result = await response.json();
            console.log('Welcome message result:', result);
            return result;
        } catch (error) {
            console.error('Error sending welcome message:', error);
            throw error;
        }
    }

    // Process a message
    async processMessage(phoneNumber, message) {
        try {
            const response = await fetch(`${this.baseUrl}/process-message`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ phoneNumber, message })
            });

            const result = await response.json();
            console.log('Process message result:', result);
            return result;
        } catch (error) {
            console.error('Error processing message:', error);
            throw error;
        }
    }

    // Test consultation flow
    async testConsultationFlow(phoneNumber) {
        console.log('Testing consultation flow...');
        
        // Step 1: Send welcome
        await this.sendWelcome(phoneNumber);
        
        // Step 2: Choose consultation
        await this.processMessage(phoneNumber, '1');
        
        // Step 3: Choose specialty
        await this.processMessage(phoneNumber, '1'); // General medicine
        
        // Step 4: Enter name
        await this.processMessage(phoneNumber, 'أحمد محمد');
        
        // Step 5: Enter phone
        await this.processMessage(phoneNumber, '+966501234567');
        
        // Step 6: Enter birth date
        await this.processMessage(phoneNumber, '1990-01-01');
        
        // Step 7: Enter birth country
        await this.processMessage(phoneNumber, 'السعودية');
        
        // Step 8: Enter current country
        await this.processMessage(phoneNumber, 'السعودية');
        
        // Step 9: Additional info
        await this.processMessage(phoneNumber, 'لا');
        
        // Step 10: Confirm
        await this.processMessage(phoneNumber, 'نعم');
        
        console.log('Consultation flow test completed');
    }

    // Test doctor join flow
    async testDoctorJoinFlow(phoneNumber) {
        console.log('Testing doctor join flow...');
        
        // Step 1: Send welcome
        await this.sendWelcome(phoneNumber);
        
        // Step 2: Choose doctor join
        await this.processMessage(phoneNumber, '2');
        
        // Step 3: Enter name
        await this.processMessage(phoneNumber, 'د. فاطمة أحمد');
        
        // Step 4: Enter email
        await this.processMessage(phoneNumber, 'fatima@example.com');
        
        // Step 5: Enter phone
        await this.processMessage(phoneNumber, '+966501234567');
        
        // Step 6: Enter license number
        await this.processMessage(phoneNumber, '12345');
        
        // Step 7: Enter experience years
        await this.processMessage(phoneNumber, '5');
        
        // Step 8: Enter education
        await this.processMessage(phoneNumber, 'دكتوراه في الطب');
        
        // Step 9: Enter biography
        await this.processMessage(phoneNumber, 'طبيبة متخصصة في طب الأطفال مع خبرة 5 سنوات');
        
        // Step 10: Confirm
        await this.processMessage(phoneNumber, 'نعم');
        
        console.log('Doctor join flow test completed');
    }

    // Test partnership flow
    async testPartnershipFlow(phoneNumber) {
        console.log('Testing partnership flow...');
        
        // Step 1: Send welcome
        await this.sendWelcome(phoneNumber);
        
        // Step 2: Choose partnership
        await this.processMessage(phoneNumber, '3');
        
        // Step 3: Enter company name
        await this.processMessage(phoneNumber, 'شركة الصحة المتقدمة');
        
        // Step 4: Enter contact person
        await this.processMessage(phoneNumber, 'محمد علي');
        
        // Step 5: Enter position
        await this.processMessage(phoneNumber, 'مدير عام');
        
        // Step 6: Enter email
        await this.processMessage(phoneNumber, 'mohamed@company.com');
        
        // Step 7: Enter phone
        await this.processMessage(phoneNumber, '+966501234567');
        
        // Step 8: Enter address
        await this.processMessage(phoneNumber, 'الرياض، المملكة العربية السعودية');
        
        // Step 9: Enter partnership type
        await this.processMessage(phoneNumber, 'شراكة طبية');
        
        // Step 10: Enter details
        await this.processMessage(phoneNumber, 'نرغب في شراكة لتقديم خدمات طبية متقدمة');
        
        // Step 11: Confirm
        await this.processMessage(phoneNumber, 'نعم');
        
        console.log('Partnership flow test completed');
    }
}

// Global instance for testing
window.whatsAppBotTester = new WhatsAppBotTester();

// Test functions for console
window.testConsultation = (phoneNumber = '+966501234567') => {
    window.whatsAppBotTester.testConsultationFlow(phoneNumber);
};

window.testDoctorJoin = (phoneNumber = '+966501234567') => {
    window.whatsAppBotTester.testDoctorJoinFlow(phoneNumber);
};

window.testPartnership = (phoneNumber = '+966501234567') => {
    window.whatsAppBotTester.testPartnershipFlow(phoneNumber);
};

window.sendWelcome = (phoneNumber = '+966501234567') => {
    window.whatsAppBotTester.sendWelcome(phoneNumber);
};

window.processMessage = (phoneNumber = '+966501234567', message = 'مرحبا') => {
    window.whatsAppBotTester.processMessage(phoneNumber, message);
};

console.log('WhatsApp Bot Tester loaded!');
console.log('Available functions:');
console.log('- testConsultation(phoneNumber)');
console.log('- testDoctorJoin(phoneNumber)');
console.log('- testPartnership(phoneNumber)');
console.log('- sendWelcome(phoneNumber)');
console.log('- processMessage(phoneNumber, message)'); 