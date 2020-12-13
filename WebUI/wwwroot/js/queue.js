var userId = getCookieValue('AnonymousUserId');
var participateButton = document.getElementById("participate-btn");
var queueCard = document.getElementById("queue-card");
var confirmBtn = document.getElementById("confirm-racer-btn");
var queueCountdownSeconds = document.getElementById("queue-countdown-seconds");
const connection = new signalR.HubConnectionBuilder().withUrl('/queuehub').configureLogging(signalR.LogLevel.Information).build();

function displayQueuePosition(position) {
  if (position == -1) return;
  participateButton.textContent = `Positie: ${position}`;
}

participateButton.addEventListener("click", () => joinTheQueue());
confirmBtn.addEventListener("click", () => racerConfirmed());

connection.on("ReceiveQueuePosition", (position) => displayQueuePosition(position));
connection.on("RequestQueuePosition", () => requestQueuePosition());
connection.on("WaitingForConfirm", () => waitingForConfirm());
connection.on("UpdateConfirmationTime", (seconds) => updateConfirmationTime(seconds));
connection.on("RemoveConfirm", () => removeConfirm());

async function updateConnectionId() {
  try {
    await connection.invoke('UpdateConnectionId', userId);
  } catch (err) {
    console.error(err);
  }
}

async function joinTheQueue() {
  console.log("joining the Queue");
  try {
    await connection.invoke('JoinTheQueue', userId);
  } catch (err) {
    console.error(err);
  }
}

async function requestQueuePosition() {
  try {
    await connection.invoke('SendQueuePosition', userId);
  } catch (err) {
    console.error(err);
  }
}

function waitingForConfirm() {
  console.log("Waiting for user to confirm to race");
  queueCard.style.display = "block";

  participateButton.disabled = true;
}

async function racerConfirmed(){
  console.log("racer confirmed")
  try {
    connection.invoke("RacerConfirmedAsync");
  } catch (e) {
    console.error(e);
  }
}

function updateConfirmationTime(seconds) {
  console.log(seconds);
  queueCountdownSeconds.innerHTML = seconds;
}

function removeConfirm() {
  console.log("user was too late");
  queueCard.style.display = "none";
  location.reload();
}



async function start() {
  try {
    await connection.start();
    console.log('SignalR Connected.');
    await updateConnectionId();
  } catch (err) {
    console.log(err);
    setTimeout(start, 5000);
  }
}

connection.onclose(start);

// Start the connection.
start();
