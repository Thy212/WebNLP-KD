const chatbotToggler = document.querySelector(".chatbot-toggler");
const closeBtn = document.querySelector(".close-btn");
const chatbox = document.querySelector(".chatbox");
const chatInput = document.querySelector(".chat-input textarea");
const sendChatBtn = document.querySelector(".chat-input span");

let userMessage = null; // Variable to store user's message
const API_KEY = "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"; 
const inputInitHeight = chatInput.scrollHeight;

const createChatLi = (message, className) => {
    // Create a chat <li> element with passed message and className
    const chatLi = document.createElement("li");
    chatLi.classList.add("chat", `${className}`);
    let chatContent = className === "outgoing" ? `<p></p>` : `<span class="material-symbols-outlined"><img src="/weblab/images/chatbot_logo.jpg" alt="" style="max-width: 30px;max-height: 30px; display: block; margin: 0 auto;"></span><p></p>`;
    chatLi.innerHTML = chatContent;
    chatLi.querySelector("p").textContent = message;
    return chatLi; // return chat <li> element
}

const generateResponse = (chatElement) => {
    const API_URL = "https://api.openai.com/v1/chat/completions";
    const messageElement = chatElement.querySelector("p");

    // Define the properties and message for the API request
    const requestOptions = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${API_KEY}`
        },
        body: JSON.stringify({
            model: "gpt-3.5-turbo",
            messages: [{role: "user", content: userMessage}],
        })
    }

    // Send POST request to API, get response and set the reponse as paragraph text
    fetch(API_URL, requestOptions).then(res => res.json()).then(data => {

        if (userMessage.toLowerCase().includes('address')) {
            messageElement.textContent = 'Room C113, Ton Duc Thang University\n19 nguyen Huu Tho street, Tan Phong ward, District 7, Ho Chi Minh City, Vietnam.';
        } 
        else if(userMessage.toLowerCase().includes('phone')) {
            messageElement.textContent = '(028) 37755046';
        }
        else if(userMessage.toLowerCase().includes('email')) {
            messageElement.textContent = '51900239@student.tdtu.edu.vn\n51900017@student.tdtu.edu.vn';
        }
        else if(userMessage.toLowerCase().includes('contact')) {
            messageElement.innerHTML = 'Contact us: <a href="/Page/Contact" target="_blank">Here</a>\nPhone: (028) 37755046\nEmail: 51900239@student.tdtu.edu.vn\nAddress: Room C113, Ton Duc Thang University, 19 Nguyen Huu Tho street, Tan Phong ward, District 7, Ho Chi Minh City, Vietnam.';
        }   
        else if(userMessage.toLowerCase().includes('course')) {
            messageElement.innerHTML = 'Course: <a href="/Page/Training" target="_blank">Read more</a>';
        }
        else if(userMessage.toLowerCase().includes('about')) {
            messageElement.innerHTML = 'About us: <a href="/Page/About" target="_blank">Here</a>\nThe Laboratory on Natural Language Processing (NLP) and Knowledge Discovery (KD) at Ton Duc Thang University researches in fundermental issues of natural language understanding, problems of natural language analysis, and knowledge discovery from textual sources. We especially focus on Vietnamese processing and aims to provide robust NLP tools for both research and commercial purposes.';
        }
        else if(userMessage.toLowerCase().includes('news')) {
            messageElement.innerHTML = 'Latest news: <a href="/Page/News" target="_blank">Read more</a>';
        }
        else if(userMessage.toLowerCase().includes('meeting')) {
            messageElement.innerHTML = 'Latest news: <a href="/Page/Training" target="_blank">Read more</a>';
        }
        else if(userMessage.toLowerCase().includes('project')) {
            messageElement.innerHTML = 'Latest news: <a href="/Page/Research" target="_blank">Read more</a>';
        }
        else if(userMessage.toLowerCase().includes('publications')) {
            messageElement.innerHTML = 'Latest news: <a href="/Page/Research" target="_blank">Read more</a>';
        }
        else if(userMessage.toLowerCase().includes('research')) {
            messageElement.innerHTML = 'Latest news: <a href="/Page/Research" target="_blank">Read more</a>';
        }
        else if(userMessage.toLowerCase().includes('introduction')) {
            messageElement.innerHTML = 'About us: <a href="/Page/About" target="_blank">Here</a>\nThe Laboratory on Natural Language Processing (NLP) and Knowledge Discovery (KD) at Ton Duc Thang University researches in fundermental issues of natural language understanding, problems of natural language analysis, and knowledge discovery from textual sources. We especially focus on Vietnamese processing and aims to provide robust NLP tools for both research and commercial purposes.';
        }
        else if(userMessage.toLowerCase().includes('people')) {
            messageElement.innerHTML = 'You can see all members of the lab in <a href="/Page/About" target="_blank">Here</a>';
        }
        else {
            messageElement.textContent = data.choices[0].message.content.trim();
        }

    }).catch(() => {
        messageElement.classList.add("error");
        messageElement.textContent = "Oops! Something went wrong. Please try again.";
    }).finally(() => chatbox.scrollTo(0, chatbox.scrollHeight));
}

const handleChat = () => {
    userMessage = chatInput.value.trim(); // Get user entered message and remove extra whitespace
    if(!userMessage) return;

    // Clear the input textarea and set its height to default
    chatInput.value = "";
    chatInput.style.height = `${inputInitHeight}px`;

    // Append the user's message to the chatbox
    chatbox.appendChild(createChatLi(userMessage, "outgoing"));
    chatbox.scrollTo(0, chatbox.scrollHeight);
    
    setTimeout(() => {
        // Display "Thinking..." message while waiting for the response
        const incomingChatLi = createChatLi("Thinking...", "incoming");
        chatbox.appendChild(incomingChatLi);
        chatbox.scrollTo(0, chatbox.scrollHeight);
        generateResponse(incomingChatLi);
    }, 600);
}

chatInput.addEventListener("input", () => {
    // Adjust the height of the input textarea based on its content
    chatInput.style.height = `${inputInitHeight}px`;
    chatInput.style.height = `${chatInput.scrollHeight}px`;
});

chatInput.addEventListener("keydown", (e) => {
    // If Enter key is pressed without Shift key and the window 
    // width is greater than 800px, handle the chat
    if(e.key === "Enter" && !e.shiftKey && window.innerWidth > 800) {
        e.preventDefault();
        handleChat();
    }
});

sendChatBtn.addEventListener("click", handleChat);
closeBtn.addEventListener("click", () => document.body.classList.remove("show-chatbot"));
chatbotToggler.addEventListener("click", () => document.body.classList.toggle("show-chatbot"));