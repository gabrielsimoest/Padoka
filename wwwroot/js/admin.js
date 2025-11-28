function checkAdminAuth() {
    const token = localStorage.getItem('padoka_token');
    const userInfo = localStorage.getItem('padoka_user');
    
    if (!token || !userInfo) {
        window.location.href = '/Auth/Login?returnUrl=' + encodeURIComponent(window.location.pathname);
        return false;
    }
    
    try {
        const user = JSON.parse(userInfo);
        
        if (user.tipo !== 1 && user.tipo !== 'Administrador') {
            alert('Acesso não autorizado. Apenas administradores podem acessar esta área.');
            window.location.href = '/Cardapio';
            return false;
        }
        
        $('#adminName').text(user.nome || 'Administrador');
        
        return true;
    } catch (e) {
        console.error('Erro ao verificar autenticação:', e);
        window.location.href = '/Auth/Login';
        return false;
    }
}

function toggleSidebar() {
    $('.admin-sidebar').toggleClass('open');
    
    if ($('.admin-sidebar').hasClass('open')) {
        $('body').append('<div class="sidebar-overlay show" onclick="toggleSidebar()"></div>');
    } else {
        $('.sidebar-overlay').remove();
    }
}

function logout() {
    localStorage.removeItem('padoka_token');
    localStorage.removeItem('padoka_user');
    localStorage.removeItem('padoka_cart');
    localStorage.removeItem('usuario');
    
    document.cookie = 'auth_token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
    document.cookie = 'padoka_token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
    
    fetch('/api/auth/logout', { method: 'POST' })
        .finally(() => {
            window.location.href = '/Auth/Login';
        });
}

function formatCurrency(value) {
    const num = parseFloat(value) || 0;
    return new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL'
    }).format(num);
}

function getStatusClass(status) {
    const statusNum = typeof status === 'string' ? statusToNumber(status) : status;
    const classes = {
        0: 'pendente',
        1: 'em-preparo',
        2: 'pronto',
        3: 'entregue',
        4: 'cancelado'
    };
    return classes[statusNum] || 'pendente';
}

function getStatusText(status) {
    const statusNum = typeof status === 'string' ? statusToNumber(status) : status;
    const texts = {
        0: 'Pendente',
        1: 'Em Preparo',
        2: 'Pronto',
        3: 'Entregue',
        4: 'Cancelado'
    };
    return texts[statusNum] || 'Desconhecido';
}

function statusToNumber(statusStr) {
    const map = {
        'pendente': 0,
        'em preparo': 1,
        'empreparo': 1,
        'pronto': 2,
        'entregue': 3,
        'cancelado': 4
    };
    return map[(statusStr || '').toLowerCase().replace(/\s+/g, '')] ?? -1;
}

function showToast(type, message) {
    if (!$('.toast-container').length) {
        $('body').append('<div class="toast-container"></div>');
    }
    
    const iconMap = {
        'success': 'check-circle',
        'error': 'exclamation-circle',
        'warning': 'exclamation-triangle'
    };
    
    const toast = $(`
        <div class="admin-toast ${type}">
            <i class="fas fa-${iconMap[type] || 'info-circle'}"></i>
            <span>${message}</span>
        </div>
    `);
    
    $('.toast-container').append(toast);
    
    setTimeout(() => {
        toast.fadeOut(300, function() {
            $(this).remove();
        });
    }, 4000);
}

$(document).on('click', '.sidebar-overlay', function() {
    toggleSidebar();
});

function updatePendingBadge() {
    const token = localStorage.getItem('padoka_token');
    
    $.ajax({
        url: '/api/admin/dashboard',
        method: 'GET',
        headers: { 'Authorization': `Bearer ${token}` },
        success: function(response) {
            if (response.sucesso && response.dados) {
                $('#pedidosPendentes').text(response.dados.pedidosPendentes || 0);
            }
        }
    });
}

setInterval(updatePendingBadge, 30000);
