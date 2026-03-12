import streamlit as st
from groq import Groq

# ---------------- PAGE CONFIG ----------------
st.set_page_config(
    page_title="CodeSmith",
    layout="wide"
)

# ---------------- SESSION STATE ----------------
if "api_key" not in st.session_state:
    st.session_state.api_key = "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"

if "chat_history" not in st.session_state:
    st.session_state.chat_history = []

if "latest_answer" not in st.session_state:
    st.session_state.latest_answer = None

# ---------------- SIDEBAR ----------------
with st.sidebar:
    st.markdown("## ⚙️ CodeSmith Options")

    answer_mode = st.selectbox(
        "Select Answer Type",
        ["Code Only", "Theory Only", "Code + Theory"]
    )

    if st.button("🗑️ Clear History"):
        st.session_state.chat_history = []
        st.session_state.latest_answer = None
        st.success("History Cleared")

    st.markdown("---")
    st.markdown("## 📜 Chat History")

    for i, chat in enumerate(st.session_state.chat_history, 1):
        with st.expander(f"Q{i}: {chat['question'][:30]}"):
            st.code(chat["answer"])

    st.markdown("---")
    st.info("👨‍💻 Welcome to CodeSmith AI")

# ---------------- MAIN UI ----------------
st.title("🤖 CodeSmith AI")

query = st.text_area(
    "Enter your question:",
    height=100,
    placeholder="Example: Enter your questions"
)

# ---------------- BUTTON ----------------
if st.button("Click here for search"):
    if not query.strip():
        st.warning("⚠️ Idea likho pehle")
    else:
        try:
            client = Groq(api_key=st.session_state.api_key)

            system_prompt = f"""
You are an expert AI like Gemini or ChatGPT.

Mode: {answer_mode}
"""

            with st.spinner("Generating response..."):
                response = client.chat.completions.create(
                    model="llama-3.1-8b-instant",
                    messages=[
                        {"role": "system", "content": system_prompt},
                        {"role": "user", "content": query}
                    ]
                )

            answer = response.choices[0].message.content

            # save history
            st.session_state.chat_history.append({
                "question": query,
                "answer": answer
            })

            # save latest answer for hand-to-hand display
            st.session_state.latest_answer = answer

            st.success("✅ Response Generated")

        except Exception as e:
            st.error(f"❌ Error: {e}")

# ---------------- HAND-TO-HAND ANSWER (NICHE) ----------------
if st.session_state.latest_answer:
    st.subheader("🧠 Answer")
    st.code(st.session_state.latest_answer)
