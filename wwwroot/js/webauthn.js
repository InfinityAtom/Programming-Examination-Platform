async function createCredential(publicKeyCredentialCreationOptions) {
    try {
        let publicKey = {
            challenge: Uint8Array.from(atob(publicKeyCredentialCreationOptions.challenge), c => c.charCodeAt(0)),
            rp: {
                name: publicKeyCredentialCreationOptions.rp.name,
                id: publicKeyCredentialCreationOptions.rp.id
            },
            user: {
                id: Uint8Array.from(atob(publicKeyCredentialCreationOptions.user.id), c => c.charCodeAt(0)),
                name: publicKeyCredentialCreationOptions.user.name,
                displayName: publicKeyCredentialCreationOptions.user.displayName
            },
            pubKeyCredParams: publicKeyCredentialCreationOptions.pubKeyCredParams,
            authenticatorSelection: publicKeyCredentialCreationOptions.authenticatorSelection,
            timeout: publicKeyCredentialCreationOptions.timeout,
            attestation: publicKeyCredentialCreationOptions.attestation
        };

        let credential = await navigator.credentials.create({ publicKey });
        return credential;
    } catch (err) {
        console.error("Error creating credential:", err);
        throw err;
    }
}
async function getAssertion() {
    const publicKeyCredentialRequestOptions = {
        challenge: new Uint8Array([/* server-generated challenge */]).buffer,
        timeout: 60000,
        rpId: window.location.hostname,
        userVerification: "required",
    };

    try {
        const credential = await navigator.credentials.get({
            publicKey: publicKeyCredentialRequestOptions
        });

        if (credential && credential.response && credential.response.userHandle) {
            const userHandle = new TextDecoder().decode(credential.response.userHandle);
            const email = userHandle; // Assuming userHandle contains the email, adjust as necessary

            // Save email to local storage
            localStorage.setItem('userEmail', email);

            return credential;
        } else {
            throw new Error('User handle is missing or invalid.');
        }
    } catch (error) {
        console.error('Error during getAssertion:', error);
        throw error;
    }
}


async function registerPasskey() {
    try {
        const userEmail = await localStorage.getItem('userEmail');
        const displayName = userEmail.split('@')[0];

        const publicKeyCredentialCreationOptions = {
            challenge: btoa(String.fromCharCode(...crypto.getRandomValues(new Uint8Array(32)))),
            rp: {
                name: "Programming Examination Platform",
                id: window.location.hostname
            },
            user: {
                id: btoa(userEmail),
                name: userEmail,
                displayName: displayName
            },
            pubKeyCredParams: [{ type: "public-key", alg: -7 }],
            authenticatorSelection: {
                authenticatorAttachment: "cross-platform",
                userVerification: "required",
                residentKey: "required"
            },
            timeout: 60000,
            attestation: "ble"
        };

        const credential = await createCredential(publicKeyCredentialCreationOptions);
        console.log("Credential created:", credential);
        window.location.href = "/complete-registration"; 
    } catch (err) {
        console.error("Error during passkey registration:", err);
    }
}
