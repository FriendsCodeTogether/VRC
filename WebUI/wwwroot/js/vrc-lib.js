function getCookieValue(cookieName) {
  var cookie = document.cookie.match('(^|;)\\s*' + cookieName + '\\s*=\\s*([^;]+)');
  return cookie ? cookie.pop() : '';
}
