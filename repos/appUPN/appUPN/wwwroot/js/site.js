// Chatbot functionality
document.addEventListener('DOMContentLoaded', function() {
    const toggle = document.getElementById('chatbot-toggle');
    const window = document.getElementById('chatbot-window');
    const close = document.getElementById('chatbot-close');
    const input = document.getElementById('chatbot-input-field');
    const sendBtn = document.getElementById('chatbot-send');
    const messages = document.getElementById('chatbot-messages');
    const messagesContainer = document.querySelector('.chatbot-messages-container');
    
    // Generar session ID único
    let sessionId = localStorage.getItem('chatSessionId');
    if (!sessionId) {
        sessionId = 'session_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
        localStorage.setItem('chatSessionId', sessionId);
    }

    // Toggle chatbot
    toggle.addEventListener('click', function() {
        const isVisible = window.style.display === 'block';
        window.style.display = isVisible ? 'none' : 'block';
        if (!isVisible) {
            input.focus();
        }
    });

    close.addEventListener('click', function() {
        window.style.display = 'none';
    });

    // Enviar mensaje
    function sendMessage() {
        const message = input.value.trim();
        if (!message) return;

        // Mostrar mensaje del usuario
        addMessage(message, 'user');
        input.value = '';

        // Mostrar indicador de escritura
        const typingIndicator = showTypingIndicator();

        // Enviar a la API
        fetch('/api/ChatBot/message', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                message: message,
                sessionId: sessionId
            })
        })
        .then(response => response.json())
        .then(data => {
            typingIndicator.remove();
            addMessage(data.message, 'bot');
        })
        .catch(error => {
            console.error('Error:', error);
            typingIndicator.remove();
            addMessage('Lo siento, ocurrió un error. Por favor intenta nuevamente.', 'bot');
        });
    }

    sendBtn.addEventListener('click', sendMessage);
    
    input.addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            sendMessage();
        }
    });

    function addMessage(text, type) {
        const messageDiv = document.createElement('div');
        messageDiv.className = `message ${type}-message`;
        messageDiv.textContent = text;
        messages.appendChild(messageDiv);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }

    function showTypingIndicator() {
        const indicator = document.createElement('div');
        indicator.className = 'typing-indicator';
        indicator.innerHTML = '<span></span><span></span><span></span>';
        messages.appendChild(indicator);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
        return indicator;
    }
});
