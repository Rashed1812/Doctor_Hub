// Custom Confirmation Modal System
document.addEventListener("DOMContentLoaded", function () {
    var input = document.querySelector("#phoneNumber");
    var iti = window.intlTelInput(input, {
        initialCountry: "auto", // يبدأ باكتشاف الدولة تلقائياً بناءً على IP المستخدم
        geoIpLookup: function (callback) {
            fetch("https://ipapi.co/json")
                .then(function (res) { return res.json(); })
                .then(function (data) { callback(data.country_code); })
                .catch(function () { callback("us"); }); // في حالة الفشل، استخدم الولايات المتحدة كافتراضي
        },
        utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/utils.js" // تأكد من المسار الصحيح
    });

    // يمكنك تخزين كائن iti في متغير عام أو في نطاق يمكن الوصول إليه لاحقاً
    // مثلاً، إذا كنت تحتاج إليه عند إرسال النموذج:
    window.itiPhoneNumber = iti;
});

class CustomConfirmation {
    constructor() {
        this.createModal();
        this.setupEventListeners();
    }

    createModal() {
        const modalHTML = `
            <div class="modal fade custom-confirmation-modal" id="customConfirmationModal" tabindex="-1" aria-labelledby="customConfirmationModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="customConfirmationModalLabel">
                                <i class="fas fa-exclamation-triangle me-2"></i>
                                تأكيد العملية
                            </h5>
                        </div>
                        <div class="modal-body">
                            <div class="confirmation-icon warning">
                                <i class="fas fa-question-circle"></i>
                            </div>
                            <div class="confirmation-message">
                                هل أنت متأكد من تنفيذ هذه العملية؟
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-custom btn-cancel" data-bs-dismiss="modal">
                                <i class="fas fa-times me-2"></i>إلغاء
                            </button>
                            <button type="button" class="btn btn-custom btn-confirm" id="confirmActionBtn">
                                <i class="fas fa-check me-2"></i>تأكيد
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        `;

        // Remove existing modal if present
        const existingModal = document.getElementById('customConfirmationModal');
        if (existingModal) {
            existingModal.remove();
        }

        // Add modal to body
        document.body.insertAdjacentHTML('beforeend', modalHTML);
        this.modal = new bootstrap.Modal(document.getElementById('customConfirmationModal'));
        this.modalElement = document.getElementById('customConfirmationModal');
    }

    setupEventListeners() {
        // Reset modal state when hidden
        this.modalElement.addEventListener('hidden.bs.modal', () => {
            this.resetModal();
        });
    }

    show(options = {}) {
        const {
            title = 'تأكيد العملية',
            message = 'هل أنت متأكد من تنفيذ هذه العملية؟',
            type = 'warning', // warning, success, danger
            confirmText = 'تأكيد',
            cancelText = 'إلغاء',
            onConfirm = () => {},
            onCancel = () => {}
        } = options;

        // Update modal content
        this.modalElement.querySelector('.modal-title').innerHTML = `
            <i class="fas ${this.getIconClass(type)} me-2"></i>
            ${title}
        `;

        const iconElement = this.modalElement.querySelector('.confirmation-icon');
        iconElement.className = `confirmation-icon ${type}`;
        iconElement.innerHTML = `<i class="fas ${this.getIconClass(type)}"></i>`;

        this.modalElement.querySelector('.confirmation-message').textContent = message;
        
        const confirmBtn = this.modalElement.querySelector('#confirmActionBtn');
        const cancelBtn = this.modalElement.querySelector('.btn-cancel');
        
        confirmBtn.innerHTML = `<i class="fas fa-check me-2"></i>${confirmText}`;
        cancelBtn.innerHTML = `<i class="fas fa-times me-2"></i>${cancelText}`;

        // Set up event handlers
        confirmBtn.onclick = () => {
            this.showLoading(confirmBtn);
            onConfirm();
        };

        cancelBtn.onclick = () => {
            onCancel();
            this.modal.hide();
        };

        // Show modal with animation
        this.modal.show();
        
        // Add pulse animation to confirm button
        setTimeout(() => {
            confirmBtn.classList.add('btn-pulse');
        }, 500);
    }

    getIconClass(type) {
        const icons = {
            warning: 'fa-exclamation-triangle',
            success: 'fa-check-circle',
            danger: 'fa-times-circle',
            info: 'fa-info-circle'
        };
        return icons[type] || icons.warning;
    }

    showLoading(button) {
        button.classList.add('btn-loading');
        button.disabled = true;
    }

    hideLoading() {
        const confirmBtn = this.modalElement.querySelector('#confirmActionBtn');
        confirmBtn.classList.remove('btn-loading');
        confirmBtn.disabled = false;
    }

    hide() {
        this.modal.hide();
    }

    resetModal() {
        const confirmBtn = this.modalElement.querySelector('#confirmActionBtn');
        confirmBtn.classList.remove('btn-loading', 'btn-pulse');
        confirmBtn.disabled = false;
    }
}

// Enhanced Toast Notification System
class CustomToast {
    static show(message, type = 'success', duration = 4000) {
        const toastId = 'toast-' + Date.now();
        const iconClass = this.getIconClass(type);
        const bgClass = this.getBgClass(type);
        
        const toastHTML = `
            <div class="toast toast-custom ${bgClass}" id="${toastId}" role="alert" aria-live="assertive" aria-atomic="true" data-bs-delay="${duration}">
                <div class="toast-header">
                    <i class="fas ${iconClass} me-2"></i>
                    <strong class="me-auto">${this.getTitle(type)}</strong>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
                <div class="toast-body">
                    ${message}
                </div>
            </div>
        `;

        // Create toast container if it doesn't exist
        let toastContainer = document.getElementById('toast-container');
        if (!toastContainer) {
            toastContainer = document.createElement('div');
            toastContainer.id = 'toast-container';
            toastContainer.className = 'toast-container position-fixed top-0 end-0 p-3';
            toastContainer.style.zIndex = '9999';
            document.body.appendChild(toastContainer);
        }

        // Add toast to container
        toastContainer.insertAdjacentHTML('beforeend', toastHTML);
        
        // Initialize and show toast
        const toastElement = document.getElementById(toastId);
        const toast = new bootstrap.Toast(toastElement);
        toast.show();

        // Remove toast element after it's hidden
        toastElement.addEventListener('hidden.bs.toast', () => {
            toastElement.remove();
        });

        return toast;
    }

    static getIconClass(type) {
        const icons = {
            success: 'fa-check-circle',
            error: 'fa-times-circle',
            warning: 'fa-exclamation-triangle',
            info: 'fa-info-circle'
        };
        return icons[type] || icons.info;
    }

    static getBgClass(type) {
        const classes = {
            success: 'bg-success',
            error: 'bg-danger',
            warning: 'bg-warning',
            info: 'bg-info'
        };
        return classes[type] || classes.info;
    }

    static getTitle(type) {
        const titles = {
            success: 'نجح',
            error: 'خطأ',
            warning: 'تحذير',
            info: 'معلومات'
        };
        return titles[type] || titles.info;
    }
}

// Initialize the confirmation system
const customConfirmation = new CustomConfirmation();

// Enhanced functions with custom confirmation
function updateConsultationStatus(id, status) {
    const statusText = status === 'Completed' ? 'تأكيد الاستشارة' : 'إلغاء الاستشارة';
    const type = status === 'Completed' ? 'success' : 'danger';
    
    customConfirmation.show({
        title: statusText,
        message: `هل أنت متأكد من ${statusText}؟ لا يمكن التراجع عن هذا الإجراء.`,
        type: type,
        confirmText: statusText,
        cancelText: 'إلغاء',
        onConfirm: () => {
            // Perform the actual update
            fetch('/Admin/UpdateConsultationStatus', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },
                body: JSON.stringify({ consultationId: id, status: status })
            })
            .then(response => response.json())
            .then(data => {
                customConfirmation.hide();
                if (data.success) {
                    CustomToast.show('تم تحديث حالة الاستشارة بنجاح', 'success');
                    setTimeout(() => location.reload(), 2000);
                } else {
                    CustomToast.show('فشل في تحديث حالة الاستشارة: ' + data.message, 'error');
                }
            })
            .catch(error => {
                customConfirmation.hide();
                console.error('Error:', error);
                CustomToast.show('حدث خطأ أثناء تحديث حالة الاستشارة.', 'error');
            });
        }
    });
}

function updatePaymentStatus(id, statusString) { // غيرت اسم البارامتر لتجنب الالتباس
    let statusValue; // هذا المتغير سيحمل القيمة الرقمية

    // تحويل القيمة النصية إلى رقمية
    if (statusString === 'Paid') {
        statusValue = 1; // القيمة الرقمية لـ PaymentStatus.Paid
    } else if (statusString === 'Pending') {
        statusValue = 0; // القيمة الرقمية لـ PaymentStatus.Pending
    } else {
        console.error('Invalid status string:', statusString);
        CustomToast.show('خطأ: حالة دفع غير صالحة.', 'error');
        return; // توقف التنفيذ إذا كانت الحالة غير صالحة
    }

    customConfirmation.show({
        title: 'تأكيد الدفع',
        message: 'هل أنت متأكد من تأكيد حالة الدفع؟ سيتم تحديث حالة الدفع إلى "مدفوع".',
        type: 'success',
        confirmText: 'تأكيد الدفع',
        cancelText: 'إلغاء',
        onConfirm: () => {
            fetch('/Admin/UpdatePaymentStatus', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },
                // هنا نرسل القيمة الرقمية بدلاً من النص
                body: JSON.stringify({ consultationId: id, status: statusValue })
            })
                .then(response => response.json())
                .then(data => {
                    customConfirmation.hide();
                    if (data.success) {
                        CustomToast.show('تم تحديث حالة الدفع بنجاح', 'success');
                        setTimeout(() => location.reload(), 2000);
                    } else {
                        CustomToast.show('فشل في تحديث حالة الدفع: ' + data.message, 'error');
                    } ل
                })
                .catch(error => {
                    customConfirmation.hide();
                    console.error('Error:', error);
                    CustomToast.show('حدث خطأ أثناء تحديث حالة الدفع.', 'error');
                });
        }
    });
}


function generateInvoice(id) {
    CustomToast.show('جاري إنشاء الفاتورة...', 'info', 2000);
    
    fetch('/Admin/GenerateInvoicePDF', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        body: JSON.stringify({ consultationId: id })
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            const blob = new Blob([data.invoiceHtml], { type: 'text/html' });
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            a.download = data.fileName;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            CustomToast.show('تم إنشاء الفاتورة بنجاح وتحميلها', 'success');
        } else {
            CustomToast.show('فشل في إنشاء الفاتورة: ' + data.message, 'error');
        }
    })
    .catch(error => {
        console.error('Error:', error);
        CustomToast.show('حدث خطأ أثناء إنشاء الفاتورة.', 'error');
    });
}

function sendWhatsApp(id) {
    CustomToast.show('جاري تحضير رسالة الواتساب...', 'info', 2000);
    
    fetch('/Admin/SendInvoiceWhatsApp', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        body: JSON.stringify({ consultationId: id })
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            CustomToast.show('تم تحضير رسالة الواتساب بنجاح', 'success');
            window.open(data.whatsappUrl, '_blank');
        } else {
            CustomToast.show('فشل في تحضير رسالة الواتساب: ' + data.message, 'error');
        }
    })
    .catch(error => {
        console.error('Error:', error);
        CustomToast.show('حدث خطأ أثناء تحضير رسالة الواتساب.', 'error');
    });

}

