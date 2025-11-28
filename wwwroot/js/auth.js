$(document).ready(function () {
    setupPasswordToggle();
    
    setupLoginForm();
    setupRegistroForm();
});

function setupPasswordToggle() {
    $('#togglePassword').on('click', function () {
        togglePasswordVisibility('#senha', this);
    });

    $('#toggleConfirmPassword').on('click', function () {
        togglePasswordVisibility('#confirmarSenha', this);
    });
}

function togglePasswordVisibility(inputSelector, button) {
    const input = $(inputSelector);
    const icon = $(button).find('i');
    
    if (input.attr('type') === 'password') {
        input.attr('type', 'text');
        icon.removeClass('fa-eye').addClass('fa-eye-slash');
    } else {
        input.attr('type', 'password');
        icon.removeClass('fa-eye-slash').addClass('fa-eye');
    }
}

function setupLoginForm() {
    $('#loginForm').on('submit', function (e) {
        e.preventDefault();
        
        const form = this;
        
        if (!form.checkValidity()) {
            e.stopPropagation();
            $(form).addClass('was-validated');
            return;
        }

        const email = $('#email').val().trim();
        const senha = $('#senha').val();

        setLoginLoading(true);
        hideAlert();

        $.ajax({
            url: '/api/auth/login',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                email: email,
                senha: senha
            }),
            success: function (response) {
                if (response.sucesso) {
                    showAlert('success', 'Login realizado com sucesso! Redirecionando...');
                    
                    localStorage.setItem('padoka_token', response.token);
                    localStorage.setItem('padoka_user', JSON.stringify(response.usuario));
                    
                    setTimeout(function () {
                        if (response.usuario.tipo === 'Administrador') {
                            window.location.href = '/Admin/Dashboard';
                        } else {
                            window.location.href = '/';
                        }
                    }, 1000);
                }
            },
            error: function (xhr) {
                const response = xhr.responseJSON;
                const mensagem = response?.mensagem || 'Erro ao realizar login. Tente novamente.';
                showAlert('danger', mensagem);
                setLoginLoading(false);
            }
        });
    });
}

function setupRegistroForm() {
    $('#registroForm').on('submit', function (e) {
        e.preventDefault();
        
        const form = this;
        
        const senha = $('#senha').val();
        const confirmarSenha = $('#confirmarSenha').val();
        
        if (senha !== confirmarSenha) {
            $('#confirmarSenha').addClass('is-invalid');
            return;
        } else {
            $('#confirmarSenha').removeClass('is-invalid');
        }
        
        if (!form.checkValidity()) {
            e.stopPropagation();
            $(form).addClass('was-validated');
            return;
        }

        const nome = $('#nome').val().trim();
        const email = $('#email').val().trim();

        setRegistroLoading(true);
        hideAlert();

        $.ajax({
            url: '/api/auth/registro',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                nome: nome,
                email: email,
                senha: senha,
                confirmarSenha: confirmarSenha
            }),
            success: function (response) {
                if (response.sucesso) {
                    showAlert('success', 'Cadastro realizado com sucesso! Redirecionando...');
                    
                    localStorage.setItem('padoka_token', response.token);
                    localStorage.setItem('padoka_user', JSON.stringify(response.usuario));
                    
                    setTimeout(function () {
                        window.location.href = '/';
                    }, 1000);
                }
            },
            error: function (xhr) {
                const response = xhr.responseJSON;
                const mensagem = response?.mensagem || 'Erro ao realizar cadastro. Tente novamente.';
                showAlert('danger', mensagem);
                setRegistroLoading(false);
            }
        });
    });

    $('#confirmarSenha').on('input', function () {
        const senha = $('#senha').val();
        const confirmarSenha = $(this).val();
        
        if (confirmarSenha && senha !== confirmarSenha) {
            $(this).addClass('is-invalid');
        } else {
            $(this).removeClass('is-invalid');
        }
    });
}

function setLoginLoading(loading) {
    const btn = $('#btnLogin');
    const btnText = btn.find('.btn-text');
    const btnLoader = btn.find('.btn-loader');
    
    if (loading) {
        btn.prop('disabled', true);
        btnText.addClass('d-none');
        btnLoader.removeClass('d-none');
    } else {
        btn.prop('disabled', false);
        btnText.removeClass('d-none');
        btnLoader.addClass('d-none');
    }
}

function setRegistroLoading(loading) {
    const btn = $('#btnRegistro');
    const btnText = btn.find('.btn-text');
    const btnLoader = btn.find('.btn-loader');
    
    if (loading) {
        btn.prop('disabled', true);
        btnText.addClass('d-none');
        btnLoader.removeClass('d-none');
    } else {
        btn.prop('disabled', false);
        btnText.removeClass('d-none');
        btnLoader.addClass('d-none');
    }
}

function showAlert(type, message) {
    const alertHtml = `
        <div class="alert alert-${type} alert-dismissible fade show" role="alert">
            <i class="fas fa-${type === 'success' ? 'check-circle' : 'exclamation-circle'} me-2"></i>
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    `;
    
    $('#alertContainer').html(alertHtml);
}

function hideAlert() {
    $('#alertContainer').empty();
}

function logout() {
    localStorage.removeItem('padoka_token');
    localStorage.removeItem('padoka_user');
    localStorage.removeItem('padoka_cart');
    localStorage.removeItem('usuario');
    
    document.cookie = 'auth_token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
    document.cookie = 'padoka_token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
    
    $.ajax({
        url: '/api/auth/logout',
        type: 'POST',
        complete: function () {
            window.location.href = '/Auth/Login';
        }
    });
}

function verificarAutenticacao(callback) {
    $.ajax({
        url: '/api/auth/verificar',
        type: 'GET',
        success: function (response) {
            callback(response.autenticado);
        },
        error: function () {
            callback(false);
        }
    });
}

function getUsuarioLogado() {
    const usuario = localStorage.getItem('usuario');
    return usuario ? JSON.parse(usuario) : null;
}
