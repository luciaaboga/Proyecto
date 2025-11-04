window.downloadEditedImage = async (imageUrl, stickers, textElements, filters, transformations, fileName) => {
    return new Promise((resolve) => {
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');

        const img = new Image();
        img.crossOrigin = 'anonymous';
        img.onload = () => {
            canvas.width = img.width;
            canvas.height = img.height;

            ctx.save();

            if (transformations.perspective !== 0 || transformations.perspectiveVertical !== 0) {
                const perspectiveX = transformations.perspective / 100;
                const perspectiveY = transformations.perspectiveVertical / 100;

                ctx.transform(
                    1,                              
                    perspectiveY * 0.2,            
                    perspectiveX * 0.2,            
                    1,                              
                    0,                              
                    0                               
                );
            }


            ctx.translate(canvas.width / 2, canvas.height / 2);
            ctx.rotate(transformations.rotation * Math.PI / 180);

            if (transformations.flipHorizontal) {
                ctx.scale(-1, 1);
            }
            if (transformations.flipVertical) {
                ctx.scale(1, -1);
            }

            ctx.translate(-canvas.width / 2, -canvas.height / 2);

            ctx.filter = `brightness(${filters.brightness}%) contrast(${filters.contrast}%) saturate(${filters.saturation}%)`;

            ctx.drawImage(img, 0, 0, canvas.width, canvas.height);

            ctx.filter = 'none';

            ctx.restore();

            stickers.forEach(sticker => {
                ctx.save();
                ctx.translate(sticker.x, sticker.y);
                ctx.scale(sticker.scale, sticker.scale);
                ctx.rotate(sticker.rotation * Math.PI / 180);

                if (sticker.content.startsWith('http')) {
                    const stickerImg = new Image();
                    stickerImg.src = sticker.content;
                    ctx.drawImage(stickerImg, -25, -25, 50, 50); 
                } else {
                    ctx.font = '48px Arial';
                    ctx.textAlign = 'center';
                    ctx.textBaseline = 'middle';
                    ctx.fillText(sticker.content, 0, 0);
                }

                ctx.restore();
            });

            textElements.forEach(textElement => {
                ctx.save();

                ctx.translate(textElement.x, textElement.y);
                ctx.scale(textElement.scale, textElement.scale);
                ctx.rotate(textElement.rotation * Math.PI / 180);

                ctx.fillStyle = textElement.color;
                ctx.font = `${textElement.isBold ? 'bold' : 'normal'} ${textElement.isItalic ? 'italic' : 'normal'} ${textElement.fontSize}px ${textElement.fontFamily}`;
                ctx.textAlign = 'left';
                ctx.textBaseline = 'top';
                ctx.fillText(textElement.text, 0, 0);

                ctx.restore();
            });

            const dataUrl = canvas.toDataURL('image/png');
            const link = document.createElement('a');
            link.href = dataUrl;
            link.download = fileName;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            resolve(dataUrl);
        };

        img.src = imageUrl;
    });
};

window.applyCrop = async (imageUrl, cropArea) => {
    return new Promise((resolve, reject) => {
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');

        const img = new Image();
        img.crossOrigin = 'anonymous';

        img.onload = () => {
            try {
                canvas.width = cropArea.width;
                canvas.height = cropArea.height;

                ctx.drawImage(
                    img,
                    cropArea.x, cropArea.y, cropArea.width, cropArea.height, 
                    0, 0, cropArea.width, cropArea.height                    
                );

                const dataUrl = canvas.toDataURL('image/png');
                resolve(dataUrl);
            } catch (error) {
                reject(error);
            }
        };

        img.onerror = () => {
            reject(new Error('Error al cargar la imagen para recortar'));
        };

        img.src = imageUrl;
    });
};

window.downloadImage = (imageUrl, fileName) => {
    const link = document.createElement('a');
    link.href = imageUrl;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

window.convertToWhatsAppSticker = async (imageUrl, stickers, textElements, filters, transformations, fileName) => {
    return new Promise((resolve, reject) => {
        console.log('Función convertToWhatsAppSticker llamada');
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');

        const img = new Image();
        img.crossOrigin = 'anonymous';

        img.onload = () => {
            try {
                const targetSize = 512;

                let width, height;
                if (img.width > img.height) {
                    width = targetSize;
                    height = (img.height * targetSize) / img.width;
                } else {
                    height = targetSize;
                    width = (img.width * targetSize) / img.height;
                }

                canvas.width = targetSize;
                canvas.height = targetSize;

                ctx.clearRect(0, 0, canvas.width, canvas.height);

                const x = (targetSize - width) / 2;
                const y = (targetSize - height) / 2;

                ctx.save();

                if (transformations.perspective !== 0 || transformations.perspectiveVertical !== 0) {
                    const perspectiveX = transformations.perspective / 100;
                    const perspectiveY = transformations.perspectiveVertical / 100;

                    ctx.transform(
                        1, perspectiveY * 0.2,
                        perspectiveX * 0.2, 1,
                        0, 0
                    );
                }

                ctx.filter = `brightness(${filters.brightness}%) contrast(${filters.contrast}%) saturate(${filters.saturation}%)`;

                ctx.drawImage(img, x, y, width, height);

                ctx.filter = 'none';
                ctx.restore();

                stickers.forEach(sticker => {
                    ctx.save();

                    const scaleX = targetSize / img.width;
                    const scaleY = targetSize / img.height;

                    const stickerX = sticker.x * scaleX;
                    const stickerY = sticker.y * scaleY;
                    const stickerScaleX = sticker.scaleX * scaleX;
                    const stickerScaleY = sticker.scaleY * scaleY;

                    ctx.translate(stickerX, stickerY);
                    ctx.scale(stickerScaleX, stickerScaleY);
                    ctx.rotate(sticker.rotation * Math.PI / 180);

                    if (sticker.content.startsWith('http')) {
                        const stickerImg = new Image();
                        stickerImg.src = sticker.content;
                        ctx.drawImage(stickerImg, -25, -25, 50, 50);
                    } else {
                        ctx.font = '48px Arial';
                        ctx.textAlign = 'center';
                        ctx.textBaseline = 'middle';
                        ctx.fillText(sticker.content, 0, 0);
                    }

                    ctx.restore();
                });

                textElements.forEach(textElement => {
                    ctx.save();

                    const scaleX = targetSize / img.width;
                    const scaleY = targetSize / img.height;

                    const textX = textElement.x * scaleX;
                    const textY = textElement.y * scaleY;
                    const textScaleX = textElement.scaleX * scaleX;
                    const textScaleY = textElement.scaleY * scaleY;

                    ctx.translate(textX, textY);
                    ctx.scale(textScaleX, textScaleY);
                    ctx.rotate(textElement.rotation * Math.PI / 180);

                    ctx.fillStyle = textElement.color;
                    ctx.font = `${textElement.isBold ? 'bold' : 'normal'} ${textElement.isItalic ? 'italic' : 'normal'} ${textElement.fontSize}px ${textElement.fontFamily}`;
                    ctx.textAlign = 'left';
                    ctx.textBaseline = 'top';
                    ctx.fillText(textElement.text, 0, 0);

                    ctx.restore();
                });

                const webpDataUrl = canvas.toDataURL('image/webp', 0.8);

                const link = document.createElement('a');
                link.href = webpDataUrl;
                link.download = fileName;
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);

                alert('¡Sticker creado exitosamente!\n\nPara usarlo en WhatsApp:\n1. Guarda la imagen en tu galería\n2. Abre WhatsApp\n3. Ve a los stickers\n4. Agrega un nuevo sticker\n5. Selecciona esta imagen');

                resolve(webpDataUrl);

            } catch (error) {
                reject(error);
            }
        };

        img.onerror = () => {
            reject(new Error('Error al cargar la imagen para convertir a sticker'));
        };

        img.src = imageUrl;
    });
};