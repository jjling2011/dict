function CreateUtils() {
    function fetch(tag, req, timeout) {
        return new Promise((resolve, reject) => {
            if (!window.chrome || !window.chrome.webview) {
                reject(new Error("not in webview2 enviroment"))
                return
            }

            const webview = window.chrome.webview
            const evType = "message"
            const handle = setTimeout(() => {
                removeHandler()
                reject(new Error("timeout"))
            }, timeout || 30000)

            function removeHandler() {
                clearTimeout(handle)
                webview.removeEventListener(evType, handler)
            }

            function handler(arg) {
                removeHandler()
                resolve(arg.data)
            }

            webview.addEventListener(evType, handler)
            const msg = JSON.stringify({ tag, req })
            webview.postMessage(msg)
        })
    }

    function isAlphabet(str) {
        if (!str) {
            return true
        }
        return /^[a-z.A-Z 0-9']+$/.test(str)
    }

    function removeAlphabet(str) {
        if (!str) {
            return str
        }
        return str.replace(/[a-z.A-Z 0-9']/gm, "")
    }

    function keepLeadingAlphabet(str) {
        if (!str) {
            return ""
        }
        const m = str.match(/^[a-z.A-Z 0-9']+/)
        return m ? m[0] : ""
    }

    function marks(kws, str) {
        const max = Number.MAX_SAFE_INTEGER
        if (!str || !kws || kws.length < 1) {
            return max
        }
        const lower = str.toLowerCase()
        const chars = [...lower]
        const len_s = chars.length
        const len_kw = kws.length

        let r = 0
        let idx_r = 0
        let idx_kw = 0
        let match = 0
        while (idx_r < len_s && idx_kw < len_kw) {
            if (chars[idx_r] === kws[idx_kw]) {
                match++
                r += idx_r
                idx_kw++
            }
            idx_r++
        }
        if (match >= len_kw) {
            return r + (len_s - match)
        }
        return max
    }

    function scrollToTop() {
        // window.scrollTo({ top: 0, behavior: "smooth" })
        window.scrollTo({ top: 0 })
    }

    return {
        scrollToTop,

        // fetch(tag: string, req: object, timeoutMs: int)
        fetch,

        // f(str)
        isAlphabet,

        // f(str)
        keepLeadingAlphabet,

        // f(kws, str)
        marks,

        // f(str)
        removeAlphabet,
    }
}

window.utils = CreateUtils()
