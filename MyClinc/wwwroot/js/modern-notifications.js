// Modern Toast Notification System
class ToastNotification {
    constructor() {
        this.container = this.createContainer();
        this.toasts = [];
    }

    createContainer() {
        let container = document.querySelector('.toast-container');
        if (!container) {
            container = document.createElement('div');
            container.className = 'toast-container';
            document.body.appendChild(container);
        }
        return container;
    }

    show(message, type = 'info', options = {}) {
        const toast = this.createToast(message, type, options);
        this.container.appendChild(toast);
        this.toasts.push(toast);

        // Trigger animation
        setTimeout(() => {
            toast.classList.add('show');
        }, 10);

        // Auto remove
        const duration = options.duration || 5000;
        if (duration > 0) {
            this.startProgressBar(toast, duration);
            setTimeout(() => {
                this.remove(toast);
            }, duration);
        }

        return toast;
    }

    createToast(message, type, options) {
        const toast = document.createElement('div');
        toast.className = `toast-notification ${type}`;

        const icon = this.getIcon(type);
        const title = options.title || this.getTitle(type);

        toast.innerHTML = `
            <div class="toast-icon">
                <i class="fas ${icon}"></i>
            </div>
            <div class="toast-content">
                <div class="toast-title">${title}</div>
                <div class="toast-message">${message}</div>
            </div>
            <button class="toast-close" onclick="toastSystem.remove(this.parentElement)">
                <i class="fas fa-times"></i>
            </button>
            <div class="toast-progress">
                <div class="toast-progress-bar"></div>
            </div>
        `;

        return toast;
    }

    getIcon(type) {
        const icons = {
            success: 'fa-check',
            error: 'fa-times',
            warning: 'fa-exclamation',
            info: 'fa-info'
        };
        return icons[type] || icons.info;
    }

    getTitle(type) {
        const titles = {
            success: 'نجح',
            error: 'خطأ',
            warning: 'تحذير',
            info: 'معلومات'
        };
        return titles[type] || titles.info;
    }

    startProgressBar(toast, duration) {
        const progressBar = toast.querySelector('.toast-progress-bar');
        if (progressBar) {
            progressBar.style.transition = `transform ${duration}ms linear`;
            progressBar.style.transform = 'scaleX(0)';
        }
    }

    remove(toast) {
        if (toast && toast.parentElement) {
            toast.classList.add('hide');
            setTimeout(() => {
                if (toast.parentElement) {
                    toast.parentElement.removeChild(toast);
                }
                const index = this.toasts.indexOf(toast);
                if (index > -1) {
                    this.toasts.splice(index, 1);
                }
            }, 400);
        }
    }

    success(message, options = {}) {
        return this.show(message, 'success', options);
    }

    error(message, options = {}) {
        return this.show(message, 'error', options);
    }

    warning(message, options = {}) {
        return this.show(message, 'warning', options);
    }

    info(message, options = {}) {
        return this.show(message, 'info', options);
    }

    clear() {
        this.toasts.forEach(toast => this.remove(toast));
    }
}

// Initialize global toast system
const toastSystem = new ToastNotification();

// Global functions for easy access
function showToast(message, type = 'info', options = {}) {
    return toastSystem.show(message, type, options);
}

function showSuccess(message, options = {}) {
    return toastSystem.success(message, options);
}

function showError(message, options = {}) {
    return toastSystem.error(message, options);
}

function showWarning(message, options = {}) {
    return toastSystem.warning(message, options);
}

function showInfo(message, options = {}) {
    return toastSystem.info(message, options);
}

// Replace old alert system
function showNotification(message, type = 'info', options = {}) {
    return showToast(message, type, options);
}

// Modern Alert Replacement
class ModernAlert {
    static replace() {
        // Replace all existing alerts with modern ones
        const alerts = document.querySelectorAll('.alert:not(.alert-modern)');
        alerts.forEach(alert => {
            if (!alert.classList.contains('alert-modern')) {
                alert.classList.add('alert-modern');
                
                // Add icon if not present
                if (!alert.querySelector('.alert-icon')) {
                    const icon = document.createElement('i');
                    icon.className = 'alert-icon fas';
                    
                    if (alert.classList.contains('alert-success')) {
                        icon.classList.add('fa-check-circle');
                    } else if (alert.classList.contains('alert-danger')) {
                        icon.classList.add('fa-exclamation-circle');
                    } else if (alert.classList.contains('alert-warning')) {
                        icon.classList.add('fa-exclamation-triangle');
                    } else if (alert.classList.contains('alert-info')) {
                        icon.classList.add('fa-info-circle');
                    }
                    
                    alert.insertBefore(icon, alert.firstChild);
                }
            }
        });
    }

    static create(message, type = 'info', options = {}) {
        const alert = document.createElement('div');
        alert.className = `alert alert-${type} alert-modern alert-dismissible fade show`;
        
        const icon = this.getAlertIcon(type);
        
        alert.innerHTML = `
            <i class="alert-icon fas ${icon}"></i>
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;
        
        return alert;
    }

    static getAlertIcon(type) {
        const icons = {
            success: 'fa-check-circle',
            danger: 'fa-exclamation-circle',
            warning: 'fa-exclamation-triangle',
            info: 'fa-info-circle'
        };
        return icons[type] || icons.info;
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    // Replace existing alerts
    ModernAlert.replace();
    
    // Override default alert function
    if (typeof window.alert === 'function') {
        window.originalAlert = window.alert;
        window.alert = function(message) {
            showInfo(message, { duration: 0 });
        };
    }
    
    // Override confirm function with modern modal (optional)
    if (typeof window.confirm === 'function') {
        window.originalConfirm = window.confirm;
        // Keep original confirm for now, can be replaced with custom modal later
    }
});

// Export for module systems
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { ToastNotification, ModernAlert, showToast, showSuccess, showError, showWarning, showInfo };
}

