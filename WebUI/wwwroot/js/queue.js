var userId = getCookieValue('AnonymousUserId');
var participateButton = document.getElementById("participate-btn");
const connection = new signalR.HubConnectionBuilder().withUrl('/queuehub').configureLogging(signalR.LogLevel.Information).build();

function displayQueuePosition(position) {
  if (position == -1) return;
  participateButton.textContent = `Positie: ${position}`;
}

participateButton.addEventListener("click", () => joinTheQueue());

connection.on("ReceiveQueuePosition", (position) => displayQueuePosition(position));
connection.on("RequestQueuePosition", () => requestQueuePosition());

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
