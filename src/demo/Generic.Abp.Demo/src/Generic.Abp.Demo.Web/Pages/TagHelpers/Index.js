function onMenuItemClick(el){
    console.log(el);
}
$(function () {

    $('.code').each((i,el)=>{
        var childNodes = el.childNodes;
        var newChilds = [];
        childNodes.forEach(node => {
            var tagName = node.tagName;
            if(!tagName) return;
            tagName = tagName.toLocaleLowerCase();
            if(tagName.indexOf('metro')<0) return;
            var attributes = [];
            Object.values(node.attributes).forEach((attr)=>{
                console.log(attr)
                attributes.push(`<span class="code-attribute">${attr.name}</span>="<span>${attr.value}</span>"`);
            })
            console.log(node);
            var newNode =`
                <div>
                    &lt;<span class="code-tag">${tagName}</span>
                    ${attributes.join(' ')}&gt;<span class="code-html">${node.innerHTML}</span>&lt;/<span  class="code-tag">${tagName}</span>&gt;</div>`
            newChilds.push(newNode);
        });
        el.innerHTML = newChilds.join('');
    })
});