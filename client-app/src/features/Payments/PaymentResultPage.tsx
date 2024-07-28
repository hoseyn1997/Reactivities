import { observer } from "mobx-react-lite";
import {
  useLocation,
  useParams,
} from "react-router-dom";
import { useStore } from "../../app/stores/store";
import { useEffect, useState } from "react";

export default observer(function PaymentResult() {
  const { username } = useParams();
  const currencLocation = useLocation();
  const [verificationcode, setVerificationCode] = useState<string>();



  const {
    paymentStore: { verifyPayment },
  } = useStore();

  useEffect(() => {
    verifyPayment(currencLocation.search.toString()).then(x => setVerificationCode(x))
  }, []);

  return (
    <div>
      <h1>
        here we have {username}
        <br />
        you payed really good and payment was ok
      </h1>
      <h2>your verification Code is : {verificationcode}</h2>
    </div>
  );
});
