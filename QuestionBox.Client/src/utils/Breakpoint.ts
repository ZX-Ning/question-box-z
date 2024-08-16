const lg = {
  width: 1024,
  height: 580 
};

export function isLg() {
  console.log(lg)
  return window.innerWidth > lg.width && window.innerHeight > lg.height;
}
