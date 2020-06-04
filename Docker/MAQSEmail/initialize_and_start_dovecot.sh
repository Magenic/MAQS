#/bin/bash

# Initialize account
mail_address=debug@localdomain.test
test_inbox=TestInbox
email_directory=/mnt/host/SeedData

# Go through each folder in $email_directory in alphabetical
# order, and create a mailbox with that directory name.
for mailbox in $(ls $email_directory)
do
    doveadm mailbox create -u $mail_address $mailbox

    # Go through each file in each folder in $email_directory
    # and add an email message.
    # NOTE: The message UIDs are assigned in alphabetical file
    # order, starting with 1.
    for email in $(ls $email_directory/$mailbox)
    do
        cat $email_directory/$mailbox/$email  | doveadm save -m $mailbox -u $mail_address
    done

    # Message UID 1 should be read by definition
    doveadm flags add -u $mail_address '\Seen' mailbox $mailbox uid 1
done

tail -fn 0 /var/log/mail.log