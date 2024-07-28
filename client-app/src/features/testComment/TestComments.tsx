import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useStore } from "../../app/stores/store";
import { Grid, Header, Image, Loader } from "semantic-ui-react";
import { formatDistanceToNow } from "date-fns";
import { Field, FieldProps, Form, Formik } from "formik";
import * as Yup from "yup";
import EmojiPicker from "emoji-picker-react";

interface Props {
  username: string;
}

export default observer(function TestComments({ username }: Props) {
  const { commentStore, userStore } = useStore();
  useEffect(() => {
    if (username) {
      commentStore.clearPrivateComments();
      commentStore.createPrivateHubConnection(username);
    }
    return () => {
      commentStore.clearPrivateComments();
    };
  }, [commentStore]);
  return (
    <Grid
      columns={2}
      style={{ backgroundColor: "#f0e2bd", borderRadies: "10%" }}
    >
      <Grid.Column width={12}>
        {commentStore.groupedPrivateComments.map(([group, comments]) => (
          <div key={group}>
            <Header>{group}</Header>
            {comments.map((comment) => (
              <div key={comment.id}>
                <div
                  style={{
                    display: "flex",
                    direction: "rtl",
                    padding: "10px",
                    alignItems: "center",
                  }}
                >
                  <Image size="mini" avatar src={comment.image} />
                  <p
                    style={{
                      marginTop: "revert",
                      paddingLeft: "10px",
                      paddingRight: "5px",
                    }}
                  >
                    {comment.displayName}
                  </p>
                  <p>{formatDistanceToNow(comment.createdAt)}</p>
                </div>
                <div
                  style={{
                    backgroundColor: "#99baf0",
                    direction: "rtl",
                    // float:'right',
                    color: "white",
                    fontSize: "15px",
                  }}
                >
                  <p style={{ padding: "10px" }}>{comment.body}</p>
                </div>
              </div>
            ))}
          </div>
        ))}
      </Grid.Column>
      <Grid.Column width={4}>
        <Formik
          onSubmit={(values, { resetForm }) =>
            commentStore
              .addPrivateComment(values, username)
              .then(() => resetForm())
          }
          initialValues={{ body: "" }}
          validationSchema={Yup.object({
            body: Yup.string().required(),
          })}
        >
          {({ isSubmitting, isValid, handleSubmit }) => (
            <Form className="ui form">
              <Field name="body">
                {(props: FieldProps) => (
                  <div style={{ position: "relative" }}>
                    <Loader active={isSubmitting} />
                    <textarea
                      placeholder="Enter your comment (Enter to submit, SIFT + Enter for new line)"
                      rows={5}
                      {...props.field}
                      onKeyDown={(e) => {
                        if (e.key === "Enter" && e.shiftKey) {
                          return;
                        } else if (e.key === "Enter" && !e.shiftKey) {
                          e.preventDefault();
                          isValid && handleSubmit();
                        }
                      }}
                    />
                  </div>
                )}
              </Field>
            </Form>
          )}
        </Formik>
        <EmojiPicker  />
      </Grid.Column>
    </Grid>
  );
});
