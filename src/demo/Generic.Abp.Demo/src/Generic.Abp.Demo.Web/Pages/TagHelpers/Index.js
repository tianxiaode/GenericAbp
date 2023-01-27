function onMenuItemClick(el){
    console.log(el);
}
$(function () {

    $('.code').each((i,el)=>{
        var innerHTML = el.innerHTML;        
        //innerHTML = innerHTML.replaceAll('&lt;', '<').replaceAll('&gt;', '>');
        innerHTML = innerHTML
            .replaceAll('\n', '</div><div>')
            .replaceAll('&lt;', '&lt;&lt;')
            .replaceAll('&gt;', '&gt;&gt;')
            .replace(/\s+([a-zA-Z][a-zA-Z0-9\-]+)="([^"]*)"/g, replacAttribute)
            .replace(/&lt;&lt;([a-zA-Z][a-zA-Z0-9\-]*)/g, replacTag)
            .replace(/&lt;&lt;\/([a-zA-Z][a-zA-Z0-9\-]*)&gt;&gt;/g, replacClosingTag)
            .replaceAll('&gt;&gt;', '&gt;');
        
        el.innerHTML = `<div>${innerHTML}</div>`;
    })

    function replacTag(match, tag) {
        return `&lt;<span class="code-tag">${tag}</span>`
    }

    function replacClosingTag(match, tag){
        return `&lt;/<span class="code-tag">${tag}</span>&gt;`
    }


    function replacAttribute(match, attr, value){
        return ` <span class="code-attribute">${attr}</span>="<span>${_.escape(value)}</span>"`
    }

});