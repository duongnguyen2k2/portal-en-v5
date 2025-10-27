


function create_UUID(){
  var sender = localStorage.getItem("sb_sender_name");
  var uuid = "";
  var dt = new Date().getTime();
  if (sender) {
    uuid = sender;
  } else {
  uuid = "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function(c) {
  var r = (dt + Math.random()*16)%16 | 0;
  dt = Math.floor(dt/16);
  return (c=="x" ? r :(r&0x3|0x8)).toString(16);
  });
  try {
    localStorage.setItem("sb_sender_name", uuid);
  }
  catch(err) {
    console.log("error", err);
  }
  }
  return uuid;
  }
  let __protocol = document.location.protocol;
  let __baseUrl = __protocol + "//livechat.vnpt.vn";
  let prefixNameLiveChat = "";
  let objPreDefineLiveChat = {
    appCode: '4b9e6913-2d80-11ed-acc5-3782503baab2',
    themes: "",
    appName: {
line1: 'Đô Thị Thông Minh',
line2: '' },
thumb: '', 
icon_bot: 'https://storage-ic.icenter.ai/smartbot-v2/chatbot_images/12282023/94698700-5459-4713-bf55-7c2d4be2cc4c.png',
senderName: create_UUID(), 
isTyping: true,
timeTyping: 1000,
isVoting: true,
styles: {
head: {
bgColor: '#53c088',
text: {
line: 1,
line1:{ 
color: '#fff', 
fontSize: '20px',
fontWeight: 400
},
line2: {
color: '#fff',
fontSize: '16px',
fontWeight: 400
}
}
},
border: { 
},
floatButton: {
bgColor: '#fff',
icon: 'https://storage-ic.icenter.ai/smartbot-v2/chatbot_images/12282023/dc64a85b-9c28-4f73-ad3f-81025ab81833.png',
width: '62',
height: '62', 
img: { 
width: '51.6%' 
} 
},
chat: {
 bg: '#F3F6F6',
 button: {
bg: '#53C088',
color: '#fff'
 },
answer: { 
bg: '#EEEEEE',
 color: '#545454',
fontSize: '12px',
},
question: {
bg: '#53C088', 
color: '#fff',
fontSize: '12px',
},
botIcon:'https://storage-ic.icenter.ai/smartbot-v2/chatbot_images/12282023/0439a22e-c009-482d-96d0-fdc7156bacb6.png'
}
}
   },
  appCodeHash = window.location.hash.substr(1);
  if (appCodeHash.length == 32) {
    objPreDefineLiveChat.appCode = + appCodeHash;
  }
  let vnpt_ai_livechat_script = document.createElement("script");
  vnpt_ai_livechat_script.id = "vnpt_ai_livechat_script";
  vnpt_ai_livechat_script.src = __baseUrl + "/vnpt_smartbot_ver2.js";
  document.body.appendChild(vnpt_ai_livechat_script);
  let vnpt_ai_livechat_stylesheet = document.createElement("link");
  vnpt_ai_livechat_stylesheet.id = "vnpt_ai_livechat_script";
  vnpt_ai_livechat_stylesheet.rel = "stylesheet";
  vnpt_ai_livechat_stylesheet.href = __baseUrl + "/vnpt_smartbot_ver2.css";
  document.body.appendChild(vnpt_ai_livechat_stylesheet);
  vnpt_ai_livechat_script.onload = function () {
    vnpt_ai_render_chatbox(objPreDefineLiveChat, __baseUrl, "livechat.vnpt.ai:443")
  }



  let hmb = `
    <div class="box-home d-block d-sm-none">
        <div class="row">
            <div class="col-md-4 col-6">
                <a href="#" class="item in3"><span>Tổng quan</span></a>
            </div>
            <div class="col-md-4 col-6">
                <a href="#" class="item in4"><span>Bộ máy</span></a>
            </div>

            <div class="col-md-4 col-6">
                <a href="/categories/du-lich-23.html" class="item in1"><span>Chuyên trang Du lịch</span></a>
            </div>
            <div class="col-md-4 col-6">
                <a href="#" class="item in2"><span>Chuyên trang Doanh nghiệp</span></a>
            </div>
            <div class="col-md-4 col-6">
                <a href="https://dichvucong.daklak.gov.vn/" class="item in5"><span>Dịch vụ công</span></a>
            </div>
            <div class="col-md-4 col-6">
                <a href="/van-ban.html" class="item in6"><span>Văn bản</span></a>
            </div>
        </div>
    </div>
  `;

  $(".col-center").before(hmb);

  
  //

  