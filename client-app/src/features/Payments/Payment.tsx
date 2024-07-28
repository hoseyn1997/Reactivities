import { observable } from "mobx";
import { observer } from "mobx-react-lite";
import { Button } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";

export default observer(function PaymentPage() {
  const {
    paymentStore: { createPaymnet },
  } = useStore();
  return (
    <div
      style={{
        backgroundColor: "white",
        width: "100%",
        height: "500px",
        textAlign: "center",
      }}
    >
      <Button
        onClick={() => createPaymnet(150000)}
        content="pay here"
        size="huge"
        positive
      />
    </div>
  );
});
