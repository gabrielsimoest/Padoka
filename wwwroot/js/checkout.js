let cart = [];

$(document).ready(function() {
    loadCart();
    setupEventListeners();
    checkAuthentication();
});

function checkAuthentication() {
    const token = localStorage.getItem('padoka_token');
    if (!token) {
        showAlert('warning', 'Voce precisa estar logado para fazer um pedido.');
        setTimeout(() => {
            window.location.href = '/Auth/Login?returnUrl=/Pedido/Checkout';
        }, 2000);
    }
}

function loadCart() {
    const savedCart = localStorage.getItem('padoka_cart');
    if (savedCart) {
        try {
            cart = JSON.parse(savedCart);
            cart = cart.map(item => normalizeCartItem(item));
        } catch (e) {
            console.error('Erro ao carregar carrinho:', e);
            cart = [];
        }
    }
    renderCart();
}

function normalizeCartItem(item) {
    const precoBase = parseFloat(item.preco) || parseFloat(item.precoUnitario) || 0;
    
    let precoComOpcoes = precoBase;
    if (item.opcoes && Array.isArray(item.opcoes)) {
        item.opcoes.forEach(opcao => {
            precoComOpcoes += parseFloat(opcao.preco || opcao.precoAdicional || 0);
        });
    }
    
    const quantidade = parseInt(item.quantidade) || 1;
    
    return {
        id: item.id || item.itemCardapioId,
        itemCardapioId: item.itemCardapioId || item.id,
        nome: item.nome || 'Item',
        preco: precoBase,
        precoUnitario: precoComOpcoes,
        imagemUrl: item.imagemUrl || '',
        quantidade: quantidade,
        opcoes: item.opcoes || [],
        observacao: item.observacao || null,
        subtotal: precoComOpcoes * quantidade
    };
}

function saveCart() {
    localStorage.setItem('padoka_cart', JSON.stringify(cart));
}

function setupEventListeners() {
}

function confirmarPedido() {
    submitOrder();
}

function renderCart() {
    const container = $('#checkoutItems');
    const loadingState = $('#loadingState');
    const emptyState = $('#emptyState');
    const checkoutContent = $('#checkoutContent');
    
    loadingState.addClass('d-none');
    
    if (cart.length === 0) {
        emptyState.removeClass('d-none');
        checkoutContent.addClass('d-none');
        return;
    }
    
    emptyState.addClass('d-none');
    checkoutContent.removeClass('d-none');
    container.empty();
    
    const placeholderImg = "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='80' height='80' viewBox='0 0 80 80'%3E%3Crect fill='%23f5e6d3' width='80' height='80'/%3E%3Ctext x='50%25' y='50%25' fill='%23c4a574' font-family='Arial' font-size='30' text-anchor='middle' dy='.3em'%3E🍞%3C/text%3E%3C/svg%3E";
    
    cart.forEach((item, index) => {
        const imgUrl = item.imagemUrl && item.imagemUrl.trim() !== '' ? item.imagemUrl : placeholderImg;
        
        const itemHtml = `
            <div class="checkout-item" data-index="${index}">
                <div class="checkout-item-image">
                    <img src="${imgUrl}" alt="${item.nome}" onerror="this.src='${placeholderImg}'">
                </div>
                <div class="checkout-item-info">
                    <div class="checkout-item-header">
                        <h4 class="checkout-item-name">${item.nome}</h4>
                        <button type="button" class="btn-remove-item" onclick="removeItem(${index})" title="Remover">
                            <i class="fas fa-trash-alt"></i>
                        </button>
                    </div>
                    ${item.opcoes && item.opcoes.length > 0 ? `
                        <div class="checkout-item-options">
                            ${item.opcoes.map(o => `<span class="option-tag">+ ${o.nome}</span>`).join('')}
                        </div>
                    ` : ''}
                    <div class="checkout-item-footer">
                        <div class="quantity-control">
                            <button type="button" onclick="updateQuantity(${index}, -1)" ${item.quantidade <= 1 ? 'disabled' : ''}>
                                <i class="fas fa-minus"></i>
                            </button>
                            <span class="qty-value">${item.quantidade}</span>
                            <button type="button" onclick="updateQuantity(${index}, 1)">
                                <i class="fas fa-plus"></i>
                            </button>
                        </div>
                        <div class="checkout-item-prices">
                            <span class="unit-price">${formatCurrency(item.precoUnitario)} un.</span>
                            <span class="subtotal-price">${formatCurrency(item.subtotal)}</span>
                        </div>
                    </div>
                </div>
            </div>
        `;
        container.append(itemHtml);
    });
    
    updateSummary();
}

function updateQuantity(index, delta) {
    const item = cart[index];
    const newQty = item.quantidade + delta;
    
    if (newQty < 1) return;
    
    item.quantidade = newQty;
    item.subtotal = item.precoUnitario * newQty;
    
    saveCart();
    renderCart();
}

function removeItem(index) {
    cart.splice(index, 1);
    saveCart();
    
    if (cart.length === 0) {
        showAlert('info', 'Carrinho vazio. Redirecionando para o cardápio...');
        setTimeout(() => {
            window.location.href = '/Cardapio';
        }, 2000);
    } else {
        renderCart();
        showAlert('info', 'Item removido do carrinho.');
    }
}

function updateSummary() {
    let subtotal = 0;
    let totalItems = 0;
    
    cart.forEach(item => {
        subtotal += parseFloat(item.subtotal) || 0;
        totalItems += parseInt(item.quantidade) || 0;
    });
    
    $('#subtotal').text(formatCurrency(subtotal));
    $('#total').text(formatCurrency(subtotal));
    $('#itemsCount').text(totalItems + ' ' + (totalItems === 1 ? 'item' : 'itens'));
    
    $('#btnConfirmar').prop('disabled', cart.length === 0);
}

function submitOrder() {
    const token = localStorage.getItem('padoka_token');
    
    if (!token) {
        showAlert('error', 'Você precisa estar logado para fazer um pedido.');
        return;
    }
    
    if (cart.length === 0) {
        showAlert('warning', 'Adicione itens ao carrinho antes de finalizar.');
        return;
    }
    
    const pedidoData = {
        numeroMesa: $('#mesa').val() || null,
        observacao: $('#observacoes').val() || null,
        itens: cart.map(item => ({
            itemCardapioId: item.itemCardapioId,
            quantidade: item.quantidade,
            observacao: item.observacao || null,
            opcoes: (item.opcoes || []).map(opcao => ({
                opcaoAdicionalId: opcao.opcaoAdicionalId
            }))
        }))
    };
    
    showLoading(true);
    
    $.ajax({
        url: '/api/pedido',
        method: 'POST',
        contentType: 'application/json',
        headers: {
            'Authorization': `Bearer ${token}`
        },
        data: JSON.stringify(pedidoData),
        success: function(response) {
            showLoading(false);
            
            if (response.sucesso) {
                window.location.href = `/Pedido/Confirmacao/${response.dados.id}`;
            } else {
                showAlert('error', response.mensagem || 'Erro ao criar pedido.');
            }
        },
        error: function(xhr) {
            showLoading(false);
            
            let errorMessage = 'Erro ao processar pedido. Tente novamente.';
            
            if (xhr.responseJSON && xhr.responseJSON.mensagem) {
                errorMessage = xhr.responseJSON.mensagem;
            } else if (xhr.status === 401) {
                errorMessage = 'Sessão expirada. Faça login novamente.';
                setTimeout(() => {
                    window.location.href = '/Auth/Login?returnUrl=/Pedido/Checkout';
                }, 2000);
            }
            
            showAlert('error', errorMessage);
        }
    });
}

function showLoading(show) {
    if (show) {
        $('body').append(`
            <div class="loading-overlay" id="loadingOverlay">
                <div class="loading-spinner">
                    <i class="fas fa-spinner"></i>
                    <p>Enviando pedido...</p>
                </div>
            </div>
        `);
    } else {
        $('#loadingOverlay').remove();
    }
}

function formatCurrency(value) {
    const num = parseFloat(value) || 0;
    return new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL'
    }).format(num);
}

function showAlert(type, message) {
    $('.checkout-alert').remove();
    
    const iconMap = {
        'success': 'check-circle',
        'error': 'exclamation-circle',
        'warning': 'exclamation-triangle',
        'info': 'info-circle'
    };
    
    const alertClass = type === 'error' ? 'danger' : type;
    
    const alertHtml = `
        <div class="alert alert-${alertClass} checkout-alert" role="alert" style="position: fixed; top: 140px; left: 50%; transform: translateX(-50%); z-index: 1050; min-width: 300px; text-align: center; box-shadow: 0 4px 15px rgba(0,0,0,0.2);">
            <i class="fas fa-${iconMap[type]} me-2"></i>${message}
        </div>
    `;
    
    $('body').append(alertHtml);
    
    setTimeout(() => {
        $('.checkout-alert').fadeOut(300, function() {
            $(this).remove();
        });
    }, 4000);
}

$(document).on('input', '#nomeCliente', function() {
    updateSummary();
});
