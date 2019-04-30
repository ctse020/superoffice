// GenerateEmailWithAttachments('test1.somail', 1, 2, 0, 0, 'attachment1.pdf,attachment2.pdf,attachment3.pdf')

function GenerateEmailWithAttachments(mailTemplateName, contactId, personId, projectId, saleId, attachmentFileNames) {
    var fb = AjaxMethodDispatcher.CallSync('Ctse.SO.AjaxTest.AjaxMethods.GenerateEmailWithAttachments', '', mailTemplateName, Number(contactId), Number(personId), Number(projectId), Number(saleId), attachmentFileNames);
    if (fb.Content != null) {
        var bytes = base64ToArrayBuffer(fb.Content);
        saveByteArray(fb.FileName, bytes);
    }
    else {
        Dialog.Information('SuperOffice', fb.Message, 'error');
    }
}

function base64ToArrayBuffer(base64) {
    var binaryString = window.atob(base64);
    var binaryLen = binaryString.length;
    var bytes = new Uint8Array(binaryLen);
    for (var i = 0; i < binaryLen; i++) {
        var ascii = binaryString.charCodeAt(i);
        bytes[i] = ascii;
    }
    return bytes;
}

function saveByteArray(fileName, bytes) {
    var blob = new Blob([bytes], { type: "octet-stream" });
    var link = document.createElement('a');
    link.setAttribute('style', 'display: none');
    link.href = window.URL.createObjectURL(blob);
    link.download = fileName;
    document.body.appendChild(link);
    //link.click();		  
    link.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true, view: window }));
    window.URL.revokeObjectURL(url);
    link.remove();
}
